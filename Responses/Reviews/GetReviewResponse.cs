namespace Hei_Hei_Api.Responses.Reviews;

public class GetReviewResponse
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public string UserFullName { get; set; }
    public int OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
}
