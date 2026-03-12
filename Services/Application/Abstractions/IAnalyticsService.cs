using Hei_Hei_Api.Responses.Analytics;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IAnalyticsService
{
    Task<List<AnimatorAnalyticsResponse>> GetAllAnimatorsAnalyticsAsync();
    Task<AnimatorAnalyticsResponse> GetAnimatorAnalyticsByIdAsync(int animatorId);
}
