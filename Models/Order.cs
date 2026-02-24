using Hei_Hei_Api.CORE;
using Hei_Hei_Api.Enums;

namespace Hei_Hei_Api.Models;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public int PackageId { get; set; }
    public Package Package { get; set; }
    public DateTime EventDate { get; set; }
    public string Address { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderAnimator> OrderAnimators { get; set; }
    public Payment Payment { get; set; }
}
