using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Helpers;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly IEmailService _emailService;

    public AuthService(AppDbContext context, IMapper mapper, IPasswordService passwordService, IJwtService jwtService, IEmailService emailService)
    {
        _context = context;
        _mapper = mapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _emailService = emailService;
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

        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        user.EmailConfirmed = false;
        user.EmailVerificationCode = code;
        user.EmailVerificationCodeExpiresAt = DateTime.UtcNow.AddMinutes(15);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(
            user.Email,
            "Verify your Hei-Hei account",
            EmailTemplates.VerificationCode(user.FullName, code)
        );

        return _mapper.Map<CreateUserResponse>(user);
    }

    public async Task VerifyEmailAsync(VerifyEmailRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (user.EmailConfirmed)
        {
            throw new InvalidOperationException("Email is already verified.");
        }

        if (user.EmailVerificationCode != request.Code)
        {
            throw new ArgumentException("Invalid verification code.");
        }

        if (user.EmailVerificationCodeExpiresAt < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Verification code has expired.");
        }

        user.EmailConfirmed = true;
        user.EmailVerificationCode = null;
        user.EmailVerificationCodeExpiresAt = null;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _emailService.SendEmailAsync(
            user.Email,
            "Welcome to Hei-Hei!",
            EmailTemplates.Welcome(user.FullName)
        );
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.Trim().ToLowerInvariant());

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }

        if (!user.EmailConfirmed)
        {
            throw new UnauthorizedAccessException("Please verify your email before logging in.");
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
