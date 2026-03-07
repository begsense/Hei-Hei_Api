using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Responses.Users;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IAuthService
{
    Task<CreateUserResponse> RegisterAsync(CreateUserRequest request);
    Task VerifyEmailAsync(VerifyEmailRequest request);
    Task<LoginUserResponse> LoginAsync(LoginUserRequest request);
}
