namespace AuthProject.API.Models.Identity;

/// <summary>
/// ������ ��� ����� �������
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