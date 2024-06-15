using Microsoft.AspNetCore.Identity;

namespace AuthProject.API.Data.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }

    public string? OtpCode { get; set; }
    public DateTime? OtpExpiryTime { get; set; }
}