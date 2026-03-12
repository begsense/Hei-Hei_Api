namespace Hei_Hei_Api.Responses.Heroes;

public class GetHeroResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Category { get; set; }
    public string Role { get; set; }
}
