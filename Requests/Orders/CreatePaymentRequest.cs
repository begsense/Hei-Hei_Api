namespace Hei_Hei_Api.Requests.Orders;

public class CreatePaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PaymentMethod { get; set; }
    public string TransactionId { get; set; }
}
