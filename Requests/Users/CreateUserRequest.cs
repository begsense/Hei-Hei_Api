namespace Hei_Hei_Api.Requests.Users;

public class CreateUserRequest
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string HomeAddress { get; set; }
}
