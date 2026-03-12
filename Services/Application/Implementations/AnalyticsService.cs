using Hei_Hei_Api.Data;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Responses.Analytics;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class AnalyticsService : IAnalyticsService
{
    private readonly AppDbContext _context;

    public AnalyticsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<AnimatorAnalyticsResponse>> GetAllAnimatorsAnalyticsAsync()
    {
        var animators = await _context.Animators
            .Include(a => a.User)
            .Include(a => a.OrderAnimators)
            .ToListAsync();

        return animators.Select(a => MapToAnalytics(a)).ToList();
    }

    public async Task<AnimatorAnalyticsResponse> GetAnimatorAnalyticsByIdAsync(int animatorId)
    {
        var animator = await _context.Animators
            .Include(a => a.User)
            .Include(a => a.OrderAnimators)
            .FirstOrDefaultAsync(a => a.Id == animatorId);

        if (animator == null)
        {
            throw new KeyNotFoundException("Animator not found.");
        }

        return MapToAnalytics(animator);
    }

    private AnimatorAnalyticsResponse MapToAnalytics(Animator animator)
    {
        var totalEarnings = animator.OrderAnimators.Sum(oa => oa.AssignedAmount);

        var paidEarnings = animator.OrderAnimators
            .Where(oa => oa.PaidToAnimator)
            .Sum(oa => oa.AssignedAmount);

        return new AnimatorAnalyticsResponse
        {
            AnimatorId = animator.Id,
            FullName = animator.User.FullName,
            Email = animator.User.Email,
            TotalOrders = animator.OrderAnimators.Count,
            TotalEarnings = totalEarnings,
            PaidEarnings = paidEarnings,
            UnpaidEarnings = totalEarnings - paidEarnings
        };
    }
}