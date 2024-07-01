namespace AuthProject.API.Services.Identity;

public class SMSService : ISMSService
{
    public async Task SendSMSAsync(string phoneNumber, string message)
    {
        await Task.Run(() => Console.WriteLine($"{phoneNumber}: {message}"));
    }
}
