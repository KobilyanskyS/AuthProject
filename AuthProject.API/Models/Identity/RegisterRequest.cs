using System.ComponentModel.DataAnnotations;

namespace AuthProject.API.Models.Identity;

/// <summary>
/// Запрос на регистрацию
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// Email
    /// </summary>
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Дата рождения
    /// </summary>
    [Required]
    [Display(Name = "Дата рождения")]
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    /// <summary>
    /// Пароль ещё раз для подтверждения
    /// </summary>
    [Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; } = null!;

    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;
}