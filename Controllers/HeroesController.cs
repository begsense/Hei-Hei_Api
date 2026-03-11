using Hei_Hei_Api.Requests.Heroes;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/heroes")]
[ApiController]
[Authorize(Roles = "Admin")]
public class HeroesController : ControllerBase
{
    private readonly IHeroService _heroService;

    public HeroesController(IHeroService heroService)
    {
        _heroService = heroService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllHeroes()
    {
        var result = await _heroService.GetAllHeroesAsync();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHeroById(int id)
    {
        var result = await _heroService.GetHeroByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateHero([FromForm] CreateHeroRequest request)
    {
        var result = await _heroService.CreateHeroAsync(request);

        return CreatedAtAction(nameof(GetHeroById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateHero(int id, UpdateHeroRequest request)
    {
        var result = await _heroService.UpdateHeroAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHero(int id)
    {
        var result = await _heroService.DeleteHeroAsync(id);

        return Ok(result);
    }

}
