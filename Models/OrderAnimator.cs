using Hei_Hei_Api.CORE;

namespace Hei_Hei_Api.Models;

public class OrderAnimator : BaseEntity
{
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int AnimatorId { get; set; }
    public Animator Animator { get; set; }
    public int HeroId { get; set; }
    public Hero Hero { get; set; }
    public decimal AssignedAmount { get; set; }
    public bool PaidToAnimator { get; set; }
    public DateTime? PaidDate { get; set; }
}
