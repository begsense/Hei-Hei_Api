using Hei_Hei_Api.CORE;

namespace Hei_Hei_Api.Models;

public class Package : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public List<Hero> Heroes { get; set; }
}
