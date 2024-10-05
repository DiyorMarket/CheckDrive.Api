using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CheckDrive.Api.Controllers;
[Route("api/smsservice")]
[ApiController]
public class SmsController(ISmsService service) : ControllerBase
{
    private readonly ISmsService _service = service
        ?? throw new ArgumentNullException(nameof(service));
    [HttpPost("sendSms")]
    public async Task<IActionResult> SendSms()
    {
        var smsMetadata = new SmsMetadata
        {
            UserName = "Abdurahmon",
            PhoneNumber = "+998934317077",
            Subject = "Salom",
            SmsType = SmsType.MessageType
        };
        await _service.SendAsync(smsMetadata);
        return Ok("jo'natildi");
    }
}
