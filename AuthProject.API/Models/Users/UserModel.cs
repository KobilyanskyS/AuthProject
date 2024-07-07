namespace AuthProject.API.Models.Users;

/// <summary>
/// Модель пользователя
/// </summary>
public class UserModel
{
    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; } = null!;
    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; } = null!;
    /// <summary>
    /// Имя пользователя (=Email)
    /// </summary>
    public string UserName { get; set; } = null!;
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = null!;
}
