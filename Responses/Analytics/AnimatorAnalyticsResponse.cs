namespace Hei_Hei_Api.Responses.Analytics;

public class AnimatorAnalyticsResponse
{
    public int AnimatorId { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int TotalOrders { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal PaidEarnings { get; set; }
    public decimal UnpaidEarnings { get; set; }
}
