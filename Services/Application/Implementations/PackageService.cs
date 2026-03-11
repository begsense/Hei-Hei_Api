using AutoMapper;
using Hei_Hei_Api.Data;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Requests.Packages;
using Hei_Hei_Api.Responses.Packages;
using Hei_Hei_Api.Services.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Services.Application.Implementations;

public class PackageService : IPackageService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PackageService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CreatePackageResponse> CreatePackageAsync(CreatePackageRequest request)
    {
        var heroes = await _context.Heroes
            .Where(h => request.HeroIds.Contains(h.Id))
            .ToListAsync();

        if (heroes.Count != request.HeroIds.Count)
        {
            throw new KeyNotFoundException("One or more heroes not found.");
        }

        var package = new Package
        {
            Name = request.Name,
            Price = request.Price,
            Description = request.Description,
            Heroes = heroes
        };

        _context.Packages.Add(package);
        await _context.SaveChangesAsync();

        return _mapper.Map<CreatePackageResponse>(package);
    }

    public async Task<GetPackageResponse> GetPackageByIdAsync(int id)
    {
        var package = await _context.Packages
            .Include(p => p.Heroes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (package == null)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        return _mapper.Map<GetPackageResponse>(package);
    }

    public async Task<List<GetPackageResponse>> GetAllPackagesAsync()
    {
        var packages = await _context.Packages
            .Include(p => p.Heroes)
            .ToListAsync();

        return _mapper.Map<List<GetPackageResponse>>(packages);
    }

    public async Task<UpdatePackageResponse> UpdatePackageAsync(int id, UpdatePackageRequest request)
    {
        var package = await _context.Packages
            .Include(p => p.Heroes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (package == null)
        {
            throw new KeyNotFoundException("Package not found.");
        }

        if (request.HeroIds != null)
        {
            var heroes = await _context.Heroes
                .Where(h => request.HeroIds.Contains(h.Id))
                .ToListAsync();

            if (heroes.Count != request.HeroIds.Count)
            {
                throw new KeyNotFoundException("One or more heroes not found.");
            }

            package.Heroes = heroes;
        }

        package.Name = request.Name ?? package.Name;
        package.Price = request.Price ?? package.Price;
        package.Description = request.Description ?? package.Description;
        package.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<UpdatePackageResponse>(package);
    }

    public async Task<DeletePackageResponse> DeletePackageAsync(int id)
    {
        var package = await _context.Packages.FindAsync(id);

        if (package == null)
            throw new KeyNotFoundException("Package not found.");

        _context.Packages.Remove(package);
        await _context.SaveChangesAsync();

        return new DeletePackageResponse
        {
            Id = id,
            Message = "Package deleted successfully."
        };
    }
}