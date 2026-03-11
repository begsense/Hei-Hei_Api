namespace Hei_Hei_Api.Responses.Orders;

public class PaymentResponse
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PaymentMethod { get; set; }
    public string Status { get; set; }
    public string TransactionId { get; set; }
    public DateTime? PaymentDate { get; set; }
}
