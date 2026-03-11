using Hei_Hei_Api.Requests.Packages;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/packages")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllPackages()
    {
        var result = await _packageService.GetAllPackagesAsync();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPackageById(int id)
    {
        var result = await _packageService.GetPackageByIdAsync(id);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePackage(CreatePackageRequest request)
    {
        var result = await _packageService.CreatePackageAsync(request);

        return CreatedAtAction(nameof(GetPackageById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePackage(int id, UpdatePackageRequest request)
    {
        var result = await _packageService.UpdatePackageAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePackage(int id)
    {
        var result = await _packageService.DeletePackageAsync(id);

        return Ok(result);
    }
}