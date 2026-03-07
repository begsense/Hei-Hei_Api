using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public AuthService(AppDbContext context, IMapper mapper, IPasswordService passwordService, IJwtService jwtService)
    {
        _context = context;
        _mapper = mapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    public async Task<CreateUserResponse> RegisterAsync(CreateUserRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var username = request.UserName.ToLowerInvariant();

        var userExists = await _context.Users
            .AnyAsync(u => u.Email == email || u.UserName == username);

        if (userExists)
        {
            throw new InvalidOperationException("Email or username already exists.");
        }

        var user = _mapper.Map<User>(request);

        user.PasswordHash = _passwordService.HashPassword(request.Password);
        user.Email = email;
        user.UserName = username;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateUserResponse>(user);
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.Trim().ToLowerInvariant());

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var isValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash);

        if (!isValid)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        var token = _jwtService.GenerateToken(user);

        return new LoginUserResponse
        {
            Token = token,
            User = _mapper.Map<GetUserResponse>(user)
        };
    }
}
