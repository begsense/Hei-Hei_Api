namespace Hei_Hei_Api.Responses.Orders;

public class OrderAnimatorResponse
{
    public int Id { get; set; }
    public int AnimatorId { get; set; }
    public string AnimatorName { get; set; }
    public int HeroId { get; set; }
    public string HeroName { get; set; }
    public decimal AssignedAmount { get; set; }
    public bool PaidToAnimator { get; set; }
    public DateTime? PaidDate { get; set; }
}
