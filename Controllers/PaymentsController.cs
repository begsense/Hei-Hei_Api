using Hei_Hei_Api.Requests.Payments;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/payments")]
[ApiController]
[Authorize(Roles = "Admin")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPayments()
    {
        var result = await _paymentService.GetAllPaymentsAsync();

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(int id)
    {
        var result = await _paymentService.GetPaymentByIdAsync(id);

        return Ok(result);
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdatePaymentStatus(int id, UpdatePaymentStatusRequest request)
    {
        var result = await _paymentService.UpdatePaymentStatusAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        var result = await _paymentService.DeletePaymentAsync(id);

        return Ok(result);
    }
}