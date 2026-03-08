using Microsoft.AspNetCore.Mvc;
using Hei_Hei_Api.Requests.Users;
using Microsoft.AspNetCore.Authorization;
using Hei_Hei_Api.Services.Application.Abstractions;

namespace Hei_Hei_Api.Controllers;

[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await _userService.GetAllUsersAsync();

        return Ok(response);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {

        var response = await _userService.GetUserByIdAsync(id);
        return Ok(response);

    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserDetails(int id, UpdateUserRequest request)
    {

        var response = await _userService.UpdateUserAsync(id, request, User);
        return Ok(response);

    }

    [Authorize]
    [HttpPut("{id}/password")]
    public async Task<IActionResult> ChangePassword(int id, UpdatePasswordRequest request)
    {

        await _userService.ChangePasswordAsync(id, request, User);
        return Ok(new { message = "Password changed successfully." });

    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> ChangeUserRole(int id, UpdateUserRoleRequest request)
    {

        var response = await _userService.ChangeUserRoleAsync(id, request, User);
        return Ok(response);

    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {

        await _userService.DeleteUserAsync(id, User);
        return NoContent();


    }
}
