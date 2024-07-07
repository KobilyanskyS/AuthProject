
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AuthProject.API.Data.Entities;
using AuthProject.API.Services.Users;
using AuthProject.API.Models.Users;
using Microsoft.Extensions.Configuration;
using AuthProject.API.Extensions;
using System.Security.Claims;

namespace AuthProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUsersService _usersRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public UserController(IUsersService usersRepository, UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _usersRepository = usersRepository;
        _userManager = userManager;
        _configuration = configuration;
    }

    //[HttpGet("getUsersInfo")]
    //public async Task<IActionResult> GetUsersInfo()
    //{
    //    var users = await _usersRepository.GetUsersInfo();
    //    return Ok(users);
    //}

    /// <summary>
    /// Получение информации о пользователе (Берётся токен из заголовков запроса и по нему идёт поиск данных о пользователе)
    /// </summary>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение (Вывод данных о пользователе)</response>
    /// <response code="400">Ошибка API</response>
    [Authorize]
    [HttpGet("getUserInfo")]
    [ProducesResponseType(typeof(UserModel), 200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUsersInfo()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return BadRequest("Authorization header is missing");
        }

        var accessToken = authorizationHeader.ToString().Split(" ").Last();
        var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            return BadRequest("Invalid token");
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
        {
            return BadRequest("Bad credentials");
        }

        var user = await _usersRepository.GetUserById(userId);
        if (user == null)
        {
            return NotFound("User not found");
        }

        return Ok(new UserModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email
        });
    }


    /// <summary>
    /// Обновление информации о пользователе (Берётся токен из заголовков запроса и по нему идёт поиск данных о пользователе)
    /// </summary>
    /// <remarks>
    /// Пример запроса:
    ///
    ///     PUT
    ///     {
    ///         "firstName": "John",
    ///         "lastName": "Doe",
    ///         "userName": "john.doe@example.com",
    ///         "phoneNumber": "+72342343443",
    ///         "email": "john.doe@example.com"
    ///     }
    ///
    /// </remarks>
    /// <returns></returns>
    /// <response code="200">Успешное выполнение (Данные о пользователе обновлены)</response>
    /// <response code="400">Ошибка API</response>
    [Authorize]
    [HttpPut("updateUserInfo")]
    public async Task<IActionResult> UpdateUserInfo(UserModel userModel)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            return BadRequest("Authorization header is missing");
        }

        var accessToken = authorizationHeader.ToString().Split(" ").Last();
        var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            return BadRequest("Invalid token");
        }

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
        {
            return BadRequest("Bad credentials");
        }

        await _usersRepository.UpdateUserAsync(userId, userModel);
        return Ok();
    }
}
