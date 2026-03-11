namespace Hei_Hei_Api.Requests.Orders;

public class CreateOrderRequest
{
    public int PackageId { get; set; }
    public DateTime EventDate { get; set; }
    public string Address { get; set; }
}
