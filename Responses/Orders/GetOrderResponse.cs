namespace Hei_Hei_Api.Responses.Orders;

public class GetOrderResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public DateTime EventDate { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public List<OrderAnimatorResponse> Animators { get; set; }
    public PaymentResponse Payment { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
