namespace Hei_Hei_Api.Responses.Reviews;

public class CreateReviewResponse
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}
