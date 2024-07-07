using System.ComponentModel.DataAnnotations;

namespace AuthProject.API.Models.Identity;

/// <summary>
/// Запрос на получение Otp на Email
/// </summary>
public class RequestOtpEmail
{
    /// <summary>
    /// Email
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
