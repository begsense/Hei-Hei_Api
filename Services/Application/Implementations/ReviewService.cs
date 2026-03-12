using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Helpers;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Reviews;
using Hei_Hei_Api.Responses.Reviews;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class ReviewService : IReviewService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ReviewService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreateReviewResponse> CreateReviewAsync(CreateReviewRequest request, ClaimsPrincipal userClaims)
    {
        var userId = GetUserHelper.GetUserId(userClaims);

        var order = await _context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == userId);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found or does not belong to you.");
        }

        var reviewExists = await _context.Reviews
            .AnyAsync(r => r.OrderId == request.OrderId && r.UserId == userId);

        if (reviewExists)
        {
            throw new InvalidOperationException("You have already reviewed this order.");
        }

        var review = new Review
        {
            OrderId = request.OrderId,
            UserId = userId,
            Rating = request.Rating,
            Comment = request.Comment
        };

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreateReviewResponse>(review);
    }

    public async Task<List<GetReviewResponse>> GetAllReviewsAsync()
    {
        var reviews = await _context.Reviews
            .Include(r => r.User)
            .ToListAsync();

        return _mapper.Map<List<GetReviewResponse>>(reviews);
    }

    public async Task<List<GetReviewResponse>> GetReviewsByOrderIdAsync(int orderId)
    {
        var orderExists = await _context.Orders.AnyAsync(o => o.Id == orderId);

        if (!orderExists)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        var reviews = await _context.Reviews
            .Include(r => r.User)
            .Where(r => r.OrderId == orderId)
            .ToListAsync();

        return _mapper.Map<List<GetReviewResponse>>(reviews);
    }

    public async Task<GetReviewResponse> GetReviewByIdAsync(int id)
    {
        var review = await _context.Reviews
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        return _mapper.Map<GetReviewResponse>(review);
    }

    public async Task<UpdateReviewResponse> UpdateReviewAsync(int id, UpdateReviewRequest request, ClaimsPrincipal userClaims)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        if (!userClaims.IsAdminOrOwner(review.UserId))
        {
            throw new UnauthorizedAccessException("You cannot update this review.");
        }

        if (request.Rating != null)
        {
            review.Rating = request.Rating.Value;
        }

        if (request.Comment != null)
        {
            review.Comment = request.Comment;
        }

        review.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateReviewResponse>(review);
    }

    public async Task DeleteReviewAsync(int id, ClaimsPrincipal userClaims)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.Id == id);

        if (review == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        if (!userClaims.IsAdminOrOwner(review.UserId))
        {
            throw new UnauthorizedAccessException("You cannot delete this review.");
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}