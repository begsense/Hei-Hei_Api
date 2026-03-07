using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Helpers;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly IEmailService _emailService;

    public UserService(AppDbContext context, IMapper mapper, IPasswordService passwordService, IEmailService emailService)
    {
        _context = context;
        _mapper = mapper;
        _passwordService = passwordService;
        _emailService = emailService;
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

        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(request.FullName))
        {
            user.FullName = request.FullName;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            user.PhoneNumber = request.PhoneNumber;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(request.HomeAddress))
        {
            user.HomeAddress = request.HomeAddress;
            isUpdated = true;
        }

        if (isUpdated)
        {
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return _mapper.Map<GetUserResponse>(user);
        }
        else
        {
            throw new InvalidOperationException("No valid fields to update.");
        }
    }

    public async Task ChangePasswordAsync(int id, UpdatePasswordRequest request, ClaimsPrincipal currentUser)
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

        if (!_passwordService.VerifyPassword(request.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Current password is incorrect.");
        }

        if (request.NewPassword != request.ConfirmNewPassword)
        {
            throw new ArgumentException("New passwords do not match.");
        }

        user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        _ = _emailService.SendEmailAsync(
            user.Email,
            "Your password was changed",
            EmailTemplates.PasswordChanged(user.FullName)
        );
    }

    public async Task<GetUserResponse> ChangeUserRoleAsync(int id, UpdateUserRoleRequest request, ClaimsPrincipal currentUser)
    {
        if (!currentUser.IsInRole("Admin"))
        {
            throw new UnauthorizedAccessException();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (!Enum.TryParse<USER_ROLE>(request.NewRole, ignoreCase: true, out var newRole))
            throw new ArgumentException($"Invalid role: {request.NewRole}");

        user.Role = newRole;
        user.UpdatedAt = DateTime.UtcNow;

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

        var email = user.Email;
        var fullName = user.FullName;

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        _ = _emailService.SendEmailAsync(
            email,
            "Your account has been deleted",
            EmailTemplates.AccountDeleted(fullName)
        );
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
