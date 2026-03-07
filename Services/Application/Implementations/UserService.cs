using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public UserService(AppDbContext context, IMapper mapper, IPasswordService passwordService)
    {
        _context = context;
        _mapper = mapper;
        _passwordService = passwordService;
    }



    public async Task<List<GetUserResponse>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();

        return _mapper.Map<List<GetUserResponse>>(users);
    }

    public async Task<GetUserResponse> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        return _mapper.Map<GetUserResponse>(user);
    }

    public async Task<GetUserResponse> UpdateUserAsync(int id, UpdateUserRequest request, ClaimsPrincipal currentUser)
    {
        if (!IsAdminOrOwner(id, currentUser))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        _mapper.Map(request, user);

        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.PasswordHash = _passwordService.HashPassword(request.Password);
        }

        user.FullName = request.FullName;
        user.PhoneNumber = request.PhoneNumber;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetUserResponse>(user);
    }

    public async Task DeleteUserAsync(int id, ClaimsPrincipal currentUser)
    {
        if (!IsAdminOrOwner(id, currentUser))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();
    }

    private bool IsAdminOrOwner(int id, ClaimsPrincipal currentUser)
    {
        var userIdFromToken = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken == null)
        {
            return false;
        }

        return currentUser.IsInRole("Admin") || userIdFromToken == id.ToString();
    }
}
