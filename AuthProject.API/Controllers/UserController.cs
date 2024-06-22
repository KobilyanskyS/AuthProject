
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthProject.API.Data.Entities;
using AuthProject.API.Services.Users;
using AuthProject.API.Models.Users;

namespace AuthProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUsersService _usersRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(IUsersService usersRepository, UserManager<ApplicationUser> userManager)
    {
        _usersRepository = usersRepository;
        _userManager = userManager;
    }

    [HttpGet("get-users-info")]
    public async Task<IActionResult> GetUsersInfo()
    {
        var users = await _usersRepository.GetUsersInfo();
        return Ok(users);
    }

    [Authorize]
    [HttpGet("get-user-info/{id}")]
    public async Task<IActionResult> GetUsersInfo([FromRoute] int id)
    {
        var managedUser = await _userManager.FindByIdAsync(Convert.ToString(id));

        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var user = await _usersRepository.GetUserById(id);
        return Ok(new UserModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        });
    }

    [HttpPut("update-user-info/{id}")]
    public async Task<IActionResult> UpdateUserInfo([FromBody] UserModel userModel, [FromRoute]int id)
    {
        await _usersRepository.UpdateUserAsync(id, userModel);
        return Ok();
    }
}
