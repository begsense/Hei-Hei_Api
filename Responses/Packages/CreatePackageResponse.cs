namespace Hei_Hei_Api.Responses.Packages;

public class CreatePackageResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<int> HeroIds { get; set; }
    public DateTime CreatedAt { get; set; }
}
