using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Heroes;
using Hei_Hei_Api.Responses.Heroes;
using Hei_Hei_Api.Services.Application.Abstractions;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class HeroService : IHeroService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IS3Service _s3Service;

    public HeroService(AppDbContext context, IMapper mapper, IS3Service s3Service)
    {
        _context = context;
        _mapper = mapper;
        _s3Service = s3Service;
    }

    public async Task<CreateHeroResponse> CreateHeroAsync(CreateHeroRequest request)
    {
        var category = Enum.Parse<HERO_CATEGORY>(request.Category, true);
        var role = Enum.Parse<HERO_ROLE>(request.Role, true);

        ValidateImage(request.Image);
        var imageUrl = await _s3Service.UploadPublicFileAsync(request.Image, "heroes");

        var hero = new Hero
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            ImageUrl = imageUrl,
            Category = category,
            Role = role
        };

        _context.Heroes.Add(hero);

        await _context.SaveChangesAsync();

        return _mapper.Map<CreateHeroResponse>(hero);
    }

    public async Task<GetHeroResponse> UpdateHeroImageAsync(int id, IFormFile file)
    {
        var hero = await _context.Heroes.FindAsync(id);

        if (hero == null)
        {
            throw new KeyNotFoundException("Hero not found.");
        }

        ValidateImage(file);

        if (!string.IsNullOrEmpty(hero.ImageUrl))
        {
            await _s3Service.DeleteFileAsync(hero.ImageUrl);
        }

        var imageUrl = await _s3Service.UploadPublicFileAsync(file, "heroes");

        hero.ImageUrl = imageUrl;
        hero.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<GetHeroResponse>(hero);
    }

    public async Task<List<GetHeroResponse>> GetAllHeroesAsync()
    {
        var heroes = await _context.Heroes.ToListAsync();

        return _mapper.Map<List<GetHeroResponse>>(heroes);
    }

    public async Task<GetHeroResponse> GetHeroByIdAsync(int id)
    {
        var hero = await _context.Heroes.FindAsync(id);

        if (hero == null)
        {
            throw new KeyNotFoundException("Hero not found.");
        }

        return _mapper.Map<GetHeroResponse>(hero);
    }

    public async Task<UpdateHeroResponse> UpdateHeroAsync(int id, UpdateHeroRequest request)
    {
        var hero = await _context.Heroes
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hero == null)
            throw new KeyNotFoundException("Hero not found.");

        HERO_CATEGORY? category = null;
        HERO_ROLE? role = null;

        if (request.Category != null)
        {
            if (!Enum.TryParse<HERO_CATEGORY>(request.Category, true, out var parsedCategory))
            {
                throw new ArgumentException($"Invalid Category: {request.Category}");
            }

            category = parsedCategory;
        }

        if (request.Role != null)
        {
            if (!Enum.TryParse<HERO_ROLE>(request.Role, true, out var parsedRole))
            {
                throw new ArgumentException($"Invalid Role: {request.Role}");
            }

            role = parsedRole;
        }

        hero.Name = request.Name ?? hero.Name;
        hero.Price = request.Price ?? hero.Price;
        hero.Description = request.Description ?? hero.Description;
        hero.Category = category ?? hero.Category;
        hero.Role = role ?? hero.Role;

        hero.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<UpdateHeroResponse>(hero);
    }

    public async Task<DeleteHeroResponse> DeleteHeroAsync(int id)
    {
        var hero = await _context.Heroes.FindAsync(id);

        if (hero == null)
        {
            throw new KeyNotFoundException("Hero not found.");
        }

        if (!string.IsNullOrEmpty(hero.ImageUrl))
        {
            await _s3Service.DeleteFileAsync(hero.ImageUrl);
        }

        _context.Heroes.Remove(hero);

        await _context.SaveChangesAsync();

        return new DeleteHeroResponse { 
            Id = id,
            Message = "Hero deleted successfully."
        };
    }

    private void ValidateImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("Image file is required.");
        }

        var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };

        if (!allowedTypes.Contains(file.ContentType.ToLower()))
        {
            throw new ArgumentException("Invalid image format. Allowed formats: JPEG, PNG, WebP.");
        }

        const long maxSize = 5 * 1024 * 1024;

        if (file.Length > maxSize)
        {
            throw new ArgumentException("Image size exceeds 5MB.");
        }
    }
}