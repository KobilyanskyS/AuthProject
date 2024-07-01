using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthProject.API.Data;
using AuthProject.API.Data.Entities;
using AuthProject.API.Extensions;
using AuthProject.API.Models.Identity;
using AuthProject.API.Services.Identity;

namespace AuthProject.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly ISMSService _smsService;

    public IdentityController(ITokenService tokenService, DataContext context, 
        UserManager<ApplicationUser> userManager, IConfiguration configuration, IEmailService emailService, ISMSService smsService)
    {
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _emailService = emailService;
        _smsService = smsService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var managedUser = await _userManager.FindByEmailAsync(request.Email);

        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);

        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user is null)
            return Unauthorized();

        var roleIds = await _context.UserRoles.Where(r => r.UserId == user.Id).Select(x => x.RoleId).ToListAsync();
        var roles = await _context.Roles.Where(x => roleIds.Contains(x.Id)).ToListAsync();

        var accessToken = _tokenService.CreateToken(user, roles);
        user.RefreshToken = _configuration.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_configuration.GetSection("Jwt:RefreshTokenValidityInDays").Get<int>());

        user.LastLoginDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new AuthResponse
        {
            Username = user.UserName!,
            Email = user.Email!,
            Token = accessToken,
            RefreshToken = user.RefreshToken
        });
    }


    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid) return BadRequest(request);

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            RegistrationDate = DateTime.UtcNow
        };
        var result = await _userManager.CreateAsync(user, request.Password);

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        if (!result.Succeeded) return BadRequest(request);

        var findUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

        if (findUser == null) throw new Exception($"User {request.Email} not found");

        await _userManager.AddToRoleAsync(findUser, RoleConsts.Member);

        return await Authenticate(new AuthRequest
        {
            Email = request.Email,
            Password = request.Password
        });
    }


    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken(TokenModel? tokenModel)
    {
        if (tokenModel is null)
        {
            return BadRequest("Invalid client request");
        }

        var accessToken = tokenModel.AccessToken;
        var refreshToken = tokenModel.RefreshToken;
        var principal = _configuration.GetPrincipalFromExpiredToken(accessToken);

        if (principal == null)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var username = principal.Identity!.Name;
        var user = await _userManager.FindByNameAsync(username!);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Invalid access token or refresh token");
        }

        var newAccessToken = _configuration.CreateToken(principal.Claims.ToList());
        var newRefreshToken = _configuration.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        return new ObjectResult(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        });
    }

    [Authorize]
    [HttpPost]
    [Route("revoke/{username}")]
    public async Task<IActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return BadRequest("Invalid user name");

        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);

        return Ok();
    }

    [Authorize]
    [HttpPost]
    [Route("revoke-all")]
    public async Task<IActionResult> RevokeAll()
    {
        var users = _userManager.Users.ToList();
        foreach (var user in users)
        {
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
        }

        return Ok();
    }

    [HttpPost]
    [Route("forgot-password-email")]
    public async Task<IActionResult> RequestOtp(RequestOtpEmail model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request");

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return BadRequest("User not found");

        var otp = GenerateOtp();
        user.OtpCode = otp;
        user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);

        await _userManager.UpdateAsync(user);

        await _emailService.SendEmailAsync(user.Email, "Код для сброса пароля", $"Ваш код для сброса пароля: {otp}");

        return Ok("OTP has been sent to your email.");
    }


    [HttpPost]
    [Route("forgot-password-sms")]
    public async Task<IActionResult> RequestOtpSMS(RequestOtpSMS model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request");

        var user = await _context.Users.SingleOrDefaultAsync(u => u.PhoneNumber == model.PhoneNumber);
        if (user == null)
            return BadRequest("User not found");

        var otp = GenerateOtp();
        user.OtpCode = otp;
        user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5);

        await _userManager.UpdateAsync(user);

        await _smsService.SendSMSAsync(model.PhoneNumber.ToString(), $"Ваш код для сброса пароля: {otp}");

        return Ok($"OTP has been sent to your phone number.\n" +
            $"------ {model.PhoneNumber}: Ваш код для сброса пароля: {otp}");
    }

    [HttpPost]
    [Route("reset-password-with-otp")]
    public async Task<IActionResult> ResetPasswordWithOtp(ResetPasswordWithOtp model)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid request");

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
            return BadRequest("User not found");

        if (user.OtpCode != model.Otp || user.OtpExpiryTime <= DateTime.UtcNow)
            return BadRequest("Invalid or expired OTP");

        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

        if (result.Succeeded)
        {
            user.OtpCode = null;
            user.OtpExpiryTime = null;
            await _userManager.UpdateAsync(user);

            return Ok("Password has been reset successfully.");
        }

        return BadRequest("Error while resetting the password.");
    }



    private string GenerateOtp()
    {
        var random = new Random();
        var otp = random.Next(100000, 999999).ToString();
        return otp;
    }



}
