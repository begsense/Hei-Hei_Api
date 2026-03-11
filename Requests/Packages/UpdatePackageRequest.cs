namespace Hei_Hei_Api.Requests.Packages;

public class UpdatePackageRequest
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
    public List<int>? HeroIds { get; set; }
}
