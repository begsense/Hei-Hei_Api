using Hei_Hei_Api.CORE;
using Hei_Hei_Api.Enums;

namespace Hei_Hei_Api.Models;

public class User : BaseEntity
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string HomeAddress { get; set; }
    public USER_ROLE Role { get; set; } = USER_ROLE.User;
    public bool EmailConfirmed { get; set; } = false;
    public string? EmailVerificationCode { get; set; }
    public DateTime? EmailVerificationCodeExpiresAt { get; set; }
    public Animator Animator { get; set; }
    public List<Order> Orders { get; set; }
}
