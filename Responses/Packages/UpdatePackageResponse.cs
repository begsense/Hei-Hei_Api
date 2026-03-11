using Hei_Hei_Api.Responses.Heroes;

namespace Hei_Hei_Api.Responses.Packages;

public class UpdatePackageResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<GetHeroResponse> Heroes { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
