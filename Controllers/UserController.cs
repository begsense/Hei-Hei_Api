using Hei_Hei_Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using AutoMapper;
using Hei_Hei_Api.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Hei_Hei_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;

    public UserController(AppDbContext context, IMapper mapper, IPasswordService passwordService, IJwtService jwtService)
    {
        _context = context;
        _mapper = mapper;
        _passwordService = passwordService;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser(CreateUserRequest request)
    {
        var email = request.Email.ToLowerInvariant();
        var username = request.UserName.ToLowerInvariant();

        var userExists = await _context.Users
            .AnyAsync(u => u.Email == email || u.UserName == username);

        if (userExists)
        {
            return BadRequest("Email or username already exists.");
        }

        var user = _mapper.Map<User>(request);

        user.PasswordHash = _passwordService.HashPassword(request.Password);
        user.Email = email;
        user.UserName = username;

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<CreateUserResponse>(user);

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLowerInvariant());

        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var isValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash);

        if (!isValid)
        {
            return Unauthorized("Invalid credentials.");
        }

        var token = _jwtService.GenerateToken(user);

        var response = new
        {
            Token = token,
            User = _mapper.Map<GetUserResponse>(user)
        };

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        var response = _mapper.Map<GetUserResponse>(user);

        return Ok(response);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, UpdateUserRequest request)
    {
        if (!IsAdminOrOwner(id))
        {
            return Forbid();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        _mapper.Map(request, user);

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.PasswordHash = _passwordService.HashPassword(request.Password);
        }

        if (!string.IsNullOrEmpty(request.FullName))
        {
            user.FullName = request.FullName;
        }

        if (!string.IsNullOrEmpty(request.PhoneNumber))
        {
            user.PhoneNumber = request.PhoneNumber;
        }

        await _context.SaveChangesAsync();

        var response = _mapper.Map<GetUserResponse>(user);

        return Ok(response);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        if (!IsAdminOrOwner(id))
        {
            return Forbid();
        }

        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        _context.Users.Remove(user);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool IsAdminOrOwner(int id)
    {
        var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken == null)
        {
            return false;
        }

        return User.IsInRole("Admin") || userIdFromToken == id.ToString();
    }
}
