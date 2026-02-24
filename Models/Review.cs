using Hei_Hei_Api.CORE;

namespace Hei_Hei_Api.Models;

public class Review : BaseEntity
{
    public int Rating { get; set; }
    public string Comment { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
