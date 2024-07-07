namespace AuthProject.API.Models.Identity;
/// <summary>
/// ������ �� �����������
/// </summary>
public class AuthRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// ������
    /// </summary>
    public string Password { get; set; } = null!;
}