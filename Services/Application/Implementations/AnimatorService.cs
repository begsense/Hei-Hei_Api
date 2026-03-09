using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Animators;
using Hei_Hei_Api.Responses.Animators;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class AnimatorService : IAnimatorService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IS3Service _s3Service;

    public AnimatorService(AppDbContext context, IMapper mapper, IS3Service s3Service)
    {
        _context = context;
        _mapper = mapper;
        _s3Service = s3Service;
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

        return _mapper.Map<AddAnimatorInfoResponse>(animator);
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

        return _mapper.Map<GetAnimatorResponse>(animator);
    }

    public async Task<GetAnimatorResponse> UploadProfileImageAsync(int id, IFormFile file, ClaimsPrincipal userClaims)
    {
        var animator = await _context.Animators
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (animator == null)
            throw new KeyNotFoundException("Animator not found.");

        if (!IsAdminOrOwner(animator.UserId, userClaims))
            throw new UnauthorizedAccessException("You cannot update this animator.");

        if (file == null || file.Length == 0)
            throw new ArgumentException("No file provided.");

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException("Only JPEG, PNG, and WebP images are allowed.");

        const long maxSize = 5 * 1024 * 1024;
        if (file.Length > maxSize)
            throw new ArgumentException("File size must not exceed 5MB.");

        var imageUrl = await _s3Service.UploadPublicFileAsync(file, "animators");

        animator.ImageUrl = imageUrl;
        animator.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetAnimatorResponse>(animator);
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
        animator.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateAnimatorResponse>(animator);
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

        return _mapper.Map<GetAnimatorResponse>(animator);
    }

    public async Task<List<GetAnimatorResponse>> GetAllAnimatorsAsync()
    {
        var animators = await _context.Animators
            .Include(a => a.User)
            .ToListAsync();

        return _mapper.Map<List<GetAnimatorResponse>>(animators);
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
}