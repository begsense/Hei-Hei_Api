using Hei_Hei_Api.CORE;
using Hei_Hei_Api.Enums;

namespace Hei_Hei_Api.Models;

public class Payment : BaseEntity
{
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string TransactionId { get; set; }
    public PaymentStatus Status { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
}
