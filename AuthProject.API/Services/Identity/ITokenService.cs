using AuthProject.API.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace AuthProject.API.Services.Identity;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, List<IdentityRole<long>> role);
}