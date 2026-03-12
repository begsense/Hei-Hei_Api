using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/analytics")]
[ApiController]
[Authorize(Roles = "Admin")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("animators")]
    public async Task<IActionResult> GetAllAnimatorsAnalytics()
    {
        var result = await _analyticsService.GetAllAnimatorsAnalyticsAsync();

        return Ok(result);
    }

    [HttpGet("animators/{id}")]
    public async Task<IActionResult> GetAnimatorAnalytics(int id)
    {
        var result = await _analyticsService.GetAnimatorAnalyticsByIdAsync(id);

        return Ok(result);
    }
}