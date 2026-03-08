using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IUserService
{
    Task<List<GetUserResponse>> GetAllUsersAsync();

    Task<GetUserResponse> GetUserByIdAsync(int id);

    Task<GetUserResponse> UpdateUserAsync(
        int id,
        UpdateUserRequest request,
        ClaimsPrincipal currentUser);

    Task ChangePasswordAsync(int id,
        UpdatePasswordRequest request,
        ClaimsPrincipal currentUser);

    Task<GetUserResponse> ChangeUserRoleAsync(int id,
        UpdateUserRoleRequest request,
        ClaimsPrincipal currentUser);

    Task DeleteUserAsync(int id, ClaimsPrincipal currentUser);
}
