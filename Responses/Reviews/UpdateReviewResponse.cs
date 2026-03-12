namespace Hei_Hei_Api.Responses.Reviews;

public class UpdateReviewResponse
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime UpdatedAt { get; set; }
}
