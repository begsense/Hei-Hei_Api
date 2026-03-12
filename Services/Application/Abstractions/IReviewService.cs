using Hei_Hei_Api.Requests.Reviews;
using Hei_Hei_Api.Responses.Reviews;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IReviewService
{
    Task<CreateReviewResponse> CreateReviewAsync(CreateReviewRequest request, ClaimsPrincipal userClaims);
    Task<List<GetReviewResponse>> GetAllReviewsAsync();
    Task<List<GetReviewResponse>> GetReviewsByOrderIdAsync(int orderId);
    Task<GetReviewResponse> GetReviewByIdAsync(int id);
    Task<UpdateReviewResponse> UpdateReviewAsync(int id, UpdateReviewRequest request, ClaimsPrincipal userClaims);
    Task DeleteReviewAsync(int id, ClaimsPrincipal userClaims);
}
