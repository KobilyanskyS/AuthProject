namespace AuthProject.API.Models.Identity;

/// <summary>
/// Ответ после авторизации
/// </summary>
public class AuthResponse
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Username { get; set; } = null!;
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Access Token
    /// </summary>
    public string Token { get; set; } = null!;
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string RefreshToken { get; set; } = null!;
}