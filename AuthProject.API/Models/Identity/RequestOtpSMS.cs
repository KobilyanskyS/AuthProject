using System.ComponentModel.DataAnnotations;

namespace AuthProject.API.Models.Identity;

/// <summary>
/// Запрос на получение Otp на телефон
/// </summary>
public class RequestOtpSMS
{
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string PhoneNumber { get; set; }
}
