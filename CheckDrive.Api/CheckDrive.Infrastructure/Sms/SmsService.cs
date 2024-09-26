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
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3MjY0OTkwMTksImlhdCI6MTcyMzkwNzAxOSwicm9sZSI6InRlc3QiLCJzaWduIjoiZmM4NTg4ZTdiZmFmMDIzYWFjMDAyYmUzMTViZWY1YmY2OWUyZThiMDdkYjQ4Nzk0ODUwY2EyZWMyN2IwN2NiNCIsInN1YiI6IjgwMjgifQ.E4SIqUEXPlbCfN39Q7UUI0P-9jzySg-0BaF9tkp9bvE");
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
