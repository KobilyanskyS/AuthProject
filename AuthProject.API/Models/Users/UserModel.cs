namespace AuthProject.API.Models.Users;

public class UserModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string PhoneNumber { get; set; }
    public string Email { get; set; } = null!;
}
