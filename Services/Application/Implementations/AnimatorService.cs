using Hei_Hei_Api.Data;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Animators;
using Hei_Hei_Api.Responses.Animators;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class AnimatorService : IAnimatorService
{
    private readonly AppDbContext _context;

    public AnimatorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AddAnimatorInfoResponse> AddAnimatorInfoAsync(
        AddAnimatorInfoRequest request,
        ClaimsPrincipal userClaims)
    {
        var userId = GetUserId(userClaims);

        var exists = await _context.Animators
            .AnyAsync(a => a.UserId == userId);

        if (exists)
        {
            throw new InvalidOperationException("Animator profile already exists.");
        }

        var animator = new Animator
        {
            Bio = request.Bio,
            UserId = userId
        };

        _context.Animators.Add(animator);
        await _context.SaveChangesAsync();

        return new AddAnimatorInfoResponse
        {
            Id = animator.Id,
            Bio = animator.Bio
        };
    }

    public async Task<GetAnimatorResponse> GetMyAnimatorProfileAsync(ClaimsPrincipal userClaims)
    {
        var userId = GetUserId(userClaims);

        var animator = await _context.Animators
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (animator == null)
        {
            throw new KeyNotFoundException("Animator profile not found.");
        }

        return MapToResponse(animator);
    }

    public async Task<UpdateAnimatorResponse> UpdateAnimatorProfileAsync(
        int id,
        UpdateAnimatorRequest request,
        ClaimsPrincipal userClaims)
    {
        var animator = await _context.Animators
            .FirstOrDefaultAsync(a => a.Id == id);

        if (animator == null)
        {
            throw new KeyNotFoundException("Animator profile not found.");
        }

        if (!IsAdminOrOwner(animator.UserId, userClaims))
        {
            throw new UnauthorizedAccessException("You cannot update this animator.");
        }

        animator.Bio = request.Bio;

        await _context.SaveChangesAsync();

        return new UpdateAnimatorResponse
        {
            Id = animator.Id,
            Bio = animator.Bio
        };
    }

    public async Task<DeleteAnimatorResponse> DeleteAnimatorAsync(int id, ClaimsPrincipal userClaims)
    {
        var animator = await _context.Animators
            .FirstOrDefaultAsync(a => a.Id == id);

        if (animator == null)
        {
            throw new KeyNotFoundException("Animator not found.");
        }

        if (!IsAdminOrOwner(animator.UserId, userClaims))
        {
            throw new UnauthorizedAccessException("You cannot delete this animator.");
        }

        _context.Animators.Remove(animator);
        await _context.SaveChangesAsync();

        return new DeleteAnimatorResponse
        {
            Id = id,
            Message = "Animator profile deleted successfully."
        };
    }

    public async Task<GetAnimatorResponse> GetAnimatorByIdAsync(int id)
    {
        var animator = await _context.Animators
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (animator == null)
        {
            throw new KeyNotFoundException("Animator not found.");
        }

        return MapToResponse(animator);
    }

    public async Task<List<GetAnimatorResponse>> GetAllAnimatorsAsync()
    {
        var animators = await _context.Animators
            .Include(a => a.User)
            .ToListAsync();

        return animators.Select(MapToResponse).ToList();
    }

    private int GetUserId(ClaimsPrincipal userClaims)
    {
        var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User not authenticated.");
        }

        return int.Parse(userIdClaim);
    }

    private bool IsAdminOrOwner(int userId, ClaimsPrincipal currentUser)
    {
        var userIdFromToken = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdFromToken == null)
        {
            return false;
        }

        return currentUser.IsInRole("Admin") || userIdFromToken == userId.ToString();
    }

    private GetAnimatorResponse MapToResponse(Animator animator)
    {
        return new GetAnimatorResponse
        {
            Email = animator.User.Email,
            UserName = animator.User.UserName,
            FullName = animator.User.FullName,
            PhoneNumber = animator.User.PhoneNumber,
            HomeAddress = animator.User.HomeAddress,
            Bio = animator.Bio
        };
    }
}