using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;
using CheckDrive.Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace CheckDrive.Infrastructure.Sms;

internal sealed class SmsService : ISmsService
{
    private readonly SmsConfigurations _options;
    private readonly HttpClient _client;

    private readonly Dictionary<SmsType, string> templateNames = new()
    {
        { SmsType.NotificationMessage, "NotificationMessageTemplate.txt" },
        { SmsType.ForgotPassword, "PasswordResetTemplate.txt" }
    };

    public SmsService(IOptions<SmsConfigurations> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.Token);
    }

    public async Task SendAsync(SmsMetadata metadata)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("917880040"), "mobile_phone");
        content.Add(new StringContent("Bu Eskiz dan test"), "message");
        content.Add(new StringContent("4546"), "from");
        content.Add(new StringContent("http://0000.uz/test.php"), "callback_url");

        var response = await _client.PostAsync(_options.ApiUrl, content);
        response.EnsureSuccessStatusCode();
    }
}
