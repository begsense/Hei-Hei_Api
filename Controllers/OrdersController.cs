using Hei_Hei_Api.Requests.Orders;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var result = await _orderService.CreateOrderAsync(request, User);

        return CreatedAtAction(nameof(GetOrderById), new { id = result.Id }, result);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var result = await _orderService.GetOrderByIdAsync(id, User);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync();

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyOrders()
    {
        var result = await _orderService.GetMyOrdersAsync(User);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusRequest request)
    {
        var result = await _orderService.UpdateOrderStatusAsync(id, request);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/animators")]
    public async Task<IActionResult> AssignAnimator(int id, AssignAnimatorRequest request)
    {
        var result = await _orderService.AssignAnimatorAsync(id, request);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id}/payment")]
    public async Task<IActionResult> CreatePayment(int id, CreatePaymentRequest request)
    {
        var result = await _orderService.CreatePaymentAsync(id, request);

        return Ok(result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var result = await _orderService.DeleteOrderAsync(id);

        return Ok(result);
    }
}