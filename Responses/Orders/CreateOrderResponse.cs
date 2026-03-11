namespace Hei_Hei_Api.Responses.Orders;

public class CreateOrderResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PackageId { get; set; }
    public string PackageName { get; set; }
    public DateTime EventDate { get; set; }
    public string Address { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt { get; set; }
}
