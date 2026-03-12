namespace Hei_Hei_Api.Requests.Reviews;

public class CreateReviewRequest
{
    public int OrderId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}
