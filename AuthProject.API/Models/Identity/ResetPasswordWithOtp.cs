using System.ComponentModel.DataAnnotations;

namespace AuthProject.API.Models.Identity;

/// <summary>
/// Запрос на сброс пароля с помощью Otp
/// </summary>
public class ResetPasswordWithOtp
{
    /// <summary>
    /// Email
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Полученный Otp-код
    /// </summary>
    [Required]
    public string Otp { get; set; }

    /// <summary>
    /// Новый пароль
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; }
}
