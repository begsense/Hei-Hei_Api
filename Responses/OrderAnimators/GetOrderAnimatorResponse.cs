namespace Hei_Hei_Api.Responses.OrderAnimators;

public class GetOrderAnimatorResponse
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int AnimatorId { get; set; }
    public string AnimatorName { get; set; }
    public string HeroName { get; set; }
    public int HeroId { get; set; }
    public decimal AssignedAmount { get; set; }
    public bool PaidToAnimator { get; set; }
    public DateTime? PaidDate { get; set; }
}
