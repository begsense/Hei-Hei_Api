using Hei_Hei_Api.Requests.Users;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {

            var response = await _authService.RegisterAsync(request);

            return CreatedAtAction(nameof(VerifyEmail), new { email = response.Email }, response);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest request)
        {

            await _authService.VerifyEmailAsync(request);

            return Ok(new { Message = "Email verified successfully. You can now log in." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserRequest request)
        {
            var response = await _authService.LoginAsync(request);

            return Ok(response);
        }
    }
}
