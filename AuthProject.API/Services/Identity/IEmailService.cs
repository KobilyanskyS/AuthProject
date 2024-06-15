namespace AuthProject.API.Services.Identity;

public interface IEmailService
{
    Task SendEmailAsync(string email, string _to, string _message);
}
