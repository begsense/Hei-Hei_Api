namespace Hei_Hei_Api.Requests.Orders;

public class AssignAnimatorRequest
{
    public int AnimatorId { get; set; }
    public int HeroId { get; set; }
    public decimal AssignedAmount { get; set; }
}
