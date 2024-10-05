using CheckDrive.Application.Interfaces;
using CheckDrive.Application.Models;
using CheckDrive.Domain.Enums;
using CheckDrive.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace CheckDrive.Infrastructure.Sms;

internal sealed class SmsService : ISmsService
{
    private readonly SmsConfigurations _options;
    private readonly HttpClient _client;

    private readonly Dictionary<SmsType, string> templateNames = new()
    {
        { SmsType.MessageType, "MessageTemplate.txt" },
        { SmsType.ForgotPassword, "PasswordResetTemplate.txt" }
    };

    public SmsService(IOptions<SmsConfigurations> options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _options = options.Value;
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_options.Token_Type, _options.Token);
    }

    public async Task SendAsync(SmsMetadata metadata)
    {
       // var template = GetTemplate(metadata.SmsType);
       // var messageContent = await BuildMessageContent(template, metadata);

        var content = new MultipartFormDataContent();
        content.Add(new StringContent(metadata.PhoneNumber), "mobile_phone");
        content.Add(new StringContent("Bu Eskiz dan test"), "message");
        content.Add(new StringContent(_options.From), "from");
        content.Add(new StringContent(_options.CallbackUrl), "callback_url");

        var response = await _client.PostAsync(_options.ApiUrl, content);
        response.EnsureSuccessStatusCode();
    }


    private string GetTemplate(SmsType smsType)
    {
        if (!templateNames.TryGetValue(smsType, out var templateName))
        {
            throw new ArgumentOutOfRangeException(nameof(smsType), $"Template for {smsType} not found.");
        }

        return Path.Combine(AppContext.BaseDirectory, "Sms/Templates", templateName);
    }

    private async Task<string> BuildMessageContent(string templatePath, SmsMetadata metadata)
    {
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template file {templatePath} not found.");
        }

        var templateContent = await File.ReadAllTextAsync(templatePath);

        var message = templateContent.Replace("{{Name}}", metadata.UserName)
                                     .Replace("{{Code}}", metadata.Code);

        return message;
    }
}
