using System.ComponentModel.DataAnnotations;

namespace AuthProject.API.Models.Identity;

public class RequestOtpSMS
{
    public string PhoneNumber { get; set; }
}
