using Hei_Hei_Api.Requests.Animators;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers
{
    [Route("api/animator")]
    [ApiController]
    public class AnimatorController : ControllerBase
    {
        private readonly IAnimatorService _animatorService;

        public AnimatorController(IAnimatorService animatorService)
        {
            _animatorService = animatorService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddAnimatorInfo(AddAnimatorInfoRequest request)
        {
            var result = await _animatorService.AddAnimatorInfoAsync(request, User);

            return CreatedAtAction(nameof(GetAnimatorById), new { id = result.Id }, result);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyAnimatorProfile()
        {
            var result = await _animatorService.GetMyAnimatorProfileAsync(User);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimatorProfile(int id, UpdateAnimatorRequest request)
        {
            var result = await _animatorService.UpdateAnimatorProfileAsync(id, request, User);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimator(int id)
        {
            var result = await _animatorService.DeleteAnimatorAsync(id, User);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnimatorById(int id)
        {
            var result = await _animatorService.GetAnimatorByIdAsync(id);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnimators()
        {
            var result = await _animatorService.GetAllAnimatorsAsync();

            return Ok(result);
        }
    }
}
