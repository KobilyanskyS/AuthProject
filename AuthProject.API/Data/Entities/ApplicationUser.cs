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

    public DateTime RegistrationDate { get; set; }

    public DateTime LastLoginDate { get; set; }

    public int? TotalPurchases { get; set; }

    public decimal? TotalAmountSpent { get; set; }

    public int? TotalProductsViewed { get; set; }

    public string? PreferredCategory { get; set; }

    public bool? IsSubscribedToNewsletter { get; set; }

    public DateTime? LastPurchaseDate { get; set; }
}