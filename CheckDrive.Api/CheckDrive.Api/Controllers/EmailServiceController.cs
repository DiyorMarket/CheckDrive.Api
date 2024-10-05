using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using CheckDrive.Infrastructure.Email.Factories;
using Microsoft.AspNetCore.WebUtilities;

namespace CheckDrive.Api.Controllers;

[Route("api/emailservices")]
[ApiController]
public class EmailServiceController : ControllerBase
{
    private readonly IEmailService _service;
    private readonly IEmailMetadataFactory _emailMetadataFactory;

    public EmailServiceController(IEmailService service, IEmailMetadataFactory metadataFactory)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _emailMetadataFactory = metadataFactory ?? throw new ArgumentNullException(nameof(metadataFactory));
    }

    [HttpPost("sendEmail")]
    public async Task<IActionResult> SendEmail()
    {
        var email = new EmailMetadata
        {
            To = "hayitovabdurahmon76@gmail.com",
            Subject = "salom",
            EmailType = EmailType.ForgotPassword,
            UserInfo = new UserInfo("firdavsjumayev320@gmail.com", "Abdurahmon")
        };

        await _service.SendAsync(email);

        return Ok("Xabar jo'natildi");
    }
    private static string GetCallbackUri(string clientUri, string token, string email)
    {
        var param = new Dictionary<string, string?>
        {
            {"token", token },
            {"email", email }
        };
        var callbackUri = QueryHelpers.AddQueryString(clientUri, param);

        return callbackUri;
    }
}