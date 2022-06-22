using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

internal class RestRksvClient : IDisposable
{
    private const string URL_SANDBOX = "https://api-sandbox-rksv.fiskaltrust.at";
    private const string URL_PROD = "https://api-rksv.fiskaltrust.at";

    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public RestRksvClient(string cashboxId, string accessToken, bool isSandbox = true)
    {
        _client = new HttpClient { BaseAddress = new Uri(isSandbox ? URL_SANDBOX : URL_PROD) };
        _client.DefaultRequestHeaders.Add("cashboxid", cashboxId);
        _client.DefaultRequestHeaders.Add("accesstoken", accessToken);
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<byte[]> CertificateAsync()
    {
        var responseContent = await _client.GetStringAsync("api/certificate");
        return Convert.FromBase64String(JsonSerializer.Deserialize<CertificateResponse>(responseContent, _jsonOptions).CertificateBase64);
    }

    public async Task<string> EchoAsync(string message)
    {
        var requestContent = JsonSerializer.Serialize(new EchoRequest { Message = message });
        var response = await _client.PostAsync("api/echo", new StringContent(requestContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<EchoResponse>(responseContent, _jsonOptions).Message;
    }

    public async Task<byte[]> SignAsync(byte[] data)
    {
        var requestContent = JsonSerializer.Serialize(new SignRequest { DataBase64 = Convert.ToBase64String(data) });
        var response = await _client.PostAsync("api/sign", new StringContent(requestContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        return Convert.FromBase64String(JsonSerializer.Deserialize<SignResponse>(responseContent, _jsonOptions).SignedDataBase64);
    }

    public async Task<string> ZDAAsync()
    {
        var responseContent = await _client.GetStringAsync("api/zda");
        return JsonSerializer.Deserialize<ZdaResponse>(responseContent, _jsonOptions).ZDA;
    }

    public void Dispose() => _client.Dispose();
}
