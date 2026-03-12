using Hei_Hei_Api.Requests.Reviews;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hei_Hei_Api.Controllers;

[Route("api/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllReviews()
    {
        var result = await _reviewService.GetAllReviewsAsync();

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetReviewById(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetReviewsByOrderId(int orderId)
    {
        var result = await _reviewService.GetReviewsByOrderIdAsync(orderId);

        return Ok(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateReview(CreateReviewRequest request)
    {
        var result = await _reviewService.CreateReviewAsync(request, User);

        return CreatedAtAction(nameof(GetReviewById), new { id = result.Id }, result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReview(int id, UpdateReviewRequest request)
    {
        var result = await _reviewService.UpdateReviewAsync(id, request, User);

        return Ok(result);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteReviewAsync(id, User);

        return NoContent();
    }
}