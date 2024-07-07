using AuthProject.API.Data.Entities;
using AuthProject.API.Data;
using Microsoft.EntityFrameworkCore;
using AuthProject.API.Models.Users;

namespace AuthProject.API.Services.Users;

public class UsersService : IUsersService
{
    private readonly DataContext _context;

    public UsersService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<ApplicationUser>> GetUsersInfo()
    {
        var records = await _context.Users.ToListAsync();

        return records;
    }

    public async Task<ApplicationUser> GetUserById(long userId)
    {
        return await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
    }

    public async Task UpdateUserAsync(long userId, UserModel userModel)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.UserName = userModel.UserName;
            user.NormalizedUserName = userModel.UserName.ToUpper();
            user.Email = userModel.Email;
            user.NormalizedEmail = userModel.Email.ToUpper();
            user.PhoneNumber = userModel.PhoneNumber;
            
            await _context.SaveChangesAsync();
        }
    }
}
