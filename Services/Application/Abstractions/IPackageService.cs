using Hei_Hei_Api.Requests.Packages;
using Hei_Hei_Api.Responses.Packages;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IPackageService
{
    Task<CreatePackageResponse> CreatePackageAsync(CreatePackageRequest request);
    Task<GetPackageResponse> GetPackageByIdAsync(int id);
    Task<List<GetPackageResponse>> GetAllPackagesAsync();
    Task<UpdatePackageResponse> UpdatePackageAsync(int id, UpdatePackageRequest request);
    Task<DeletePackageResponse> DeletePackageAsync(int id);
}
