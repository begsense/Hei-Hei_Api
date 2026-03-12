using Hei_Hei_Api.Requests.OrderAnimators;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/orders/{orderId}/animators")]
[ApiController]
[Authorize(Roles = "Admin")]
public class OrderAnimatorsController : ControllerBase
{
    private readonly IOrderAnimatorService _orderAnimatorService;

    public OrderAnimatorsController(IOrderAnimatorService orderAnimatorService)
    {
        _orderAnimatorService = orderAnimatorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllByOrderId(int orderId)
    {
        var result = await _orderAnimatorService.GetAllByOrderIdAsync(orderId);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int orderId, int id)
    {
        var result = await _orderAnimatorService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int orderId, int id, UpdateOrderAnimatorRequest request)
    {
        var result = await _orderAnimatorService.UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int orderId, int id)
    {
        await _orderAnimatorService.DeleteAsync(id);

        return NoContent();
    }
}