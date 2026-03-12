namespace Hei_Hei_Api.Responses.OrderAnimators;

public class UpdateOrderAnimatorResponse
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int AnimatorId { get; set; }
    public decimal AssignedAmount { get; set; }
    public bool PaidToAnimator { get; set; }
    public DateTime? PaidDate { get; set; }
}
