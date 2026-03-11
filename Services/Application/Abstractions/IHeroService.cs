using Hei_Hei_Api.Requests.Heroes;
using Hei_Hei_Api.Responses.Heroes;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Hei_Hei_Api.Services.Application.Abstractions;

public interface IHeroService
{
    Task<CreateHeroResponse> CreateHeroAsync(CreateHeroRequest request);
    Task<List<GetHeroResponse>> GetAllHeroesAsync();
    Task<GetHeroResponse> GetHeroByIdAsync(int id);
    Task<UpdateHeroResponse> UpdateHeroAsync(int id, UpdateHeroRequest request);
    Task<DeleteHeroResponse> DeleteHeroAsync(int id);
    Task<GetHeroResponse> UpdateHeroImageAsync(int id, IFormFile file);
}
