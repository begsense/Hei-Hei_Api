namespace Hei_Hei_Api.Requests.Users;

public class UpdatePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassword { get; set; }
}
