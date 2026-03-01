using Hei_Hei_Api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using AutoMapper;
using Hei_Hei_Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPasswordService _passwordService;

    public UserController(AppDbContext context, IMapper mapper, IPasswordService passwordService)
    {
        _context = context;
        _mapper = mapper;
        _passwordService = passwordService;
    }

    [HttpPost("create-user")]
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

    [HttpPost("login-user")]
    public async Task<IActionResult> LoginUser(LoginUserRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email.ToLower());

        if (user == null)
            return Unauthorized("Invalid credentials.");

        var isValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash);

        if (!isValid)
            return Unauthorized("Invalid credentials.");

        var response = _mapper.Map<LoginUserResponse>(user);

        return Ok(response);
    }
}
