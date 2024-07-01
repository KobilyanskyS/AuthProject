namespace AuthProject.API.Services.Identity;

public interface ISMSService
{
    Task SendSMSAsync(string phoneNumber, string message);
}
