using Hei_Hei_Api.CORE;

namespace Hei_Hei_Api.Models;

public class Animator : BaseEntity
{
    public string Bio { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public List<OrderAnimator> OrderAnimators { get; set; }
}
