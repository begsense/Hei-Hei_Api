using Hei_Hei_Api.CORE;
using Hei_Hei_Api.Enums;

namespace Hei_Hei_Api.Models;

public class Hero : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public HeroCategory Category { get; set; }
    public HeroRole Role { get; set; }
}
