namespace AuthProject.API.Models.Identity;

/// <summary>
/// Модель для ввода токенов
/// </summary>
public class TokenModel
{
    /// <summary>
    /// Access Token
    /// </summary>
    public string? AccessToken { get; set; }
    /// <summary>
    /// Refresh Token
    /// </summary>
    public string? RefreshToken { get; set; }
}