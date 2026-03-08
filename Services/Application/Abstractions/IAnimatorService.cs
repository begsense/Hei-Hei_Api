using Hei_Hei_Api.Requests.Animators;
using Hei_Hei_Api.Responses.Animators;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IAnimatorService
{
    Task<AddAnimatorInfoResponse> AddAnimatorInfoAsync(
        AddAnimatorInfoRequest request,
        ClaimsPrincipal userClaims);

    Task<GetAnimatorResponse> GetMyAnimatorProfileAsync(
        ClaimsPrincipal userClaims);

    Task<GetAnimatorResponse> GetAnimatorByIdAsync(int id);

    Task<List<GetAnimatorResponse>> GetAllAnimatorsAsync();

    Task<UpdateAnimatorResponse> UpdateAnimatorProfileAsync(
        int id,
        UpdateAnimatorRequest request,
        ClaimsPrincipal userClaims);

    Task<DeleteAnimatorResponse> DeleteAnimatorAsync(
        int id,
        ClaimsPrincipal userClaims);
}
