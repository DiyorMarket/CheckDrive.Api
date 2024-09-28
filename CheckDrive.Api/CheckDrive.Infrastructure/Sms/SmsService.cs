using CheckDrive.Infrastructure.Models;
using CheckDrive.Infrastructure.Sms.Interface;
using System.Net.Http.Headers;

namespace CheckDrive.Infrastructure.Sms;

public class SmsService : ISmsService
{
    private readonly HttpClient _client;

    public SmsService()
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "There is should be token");
    }

    public async Task SendAsync(SmsMetadata metadata)
    {
        var content = new MultipartFormDataContent();
        content.Add(new StringContent("934317077"), "mobile_phone");
        content.Add(new StringContent("Bu Eskiz dan test"), "message");
        content.Add(new StringContent("4546"), "from");
        content.Add(new StringContent("http://0000.uz/test.php"), "callback_url");

        var response = await _client.PostAsync("https://notify.eskiz.uz/api/message/sms/send", content);
        response.EnsureSuccessStatusCode();
    }
}
