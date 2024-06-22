using AuthProject.API.Data.Entities;
using AuthProject.API.Models.Users;

namespace AuthProject.API.Services.Users;

public interface IUsersService
{
    Task<List<ApplicationUser>> GetUsersInfo();
    Task<ApplicationUser> GetUserById(long userId);
    Task UpdateUserAsync(long userId, UserModel userModel);
}