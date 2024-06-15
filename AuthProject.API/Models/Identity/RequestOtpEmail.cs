using System.ComponentModel.DataAnnotations;

namespace AuthProject.API.Models.Identity;

public class RequestOtpEmail
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
