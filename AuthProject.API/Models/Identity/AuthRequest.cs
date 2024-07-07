namespace AuthProject.API.Models.Identity;
/// <summary>
/// Запрос на авторизацию
/// </summary>
public class AuthRequest
{
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = null!;
}