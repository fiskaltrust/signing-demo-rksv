using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

internal class RestRksvClient : IDisposable
{
    private const string URL_SANDBOX = "https://rksv-sandbox.fiskaltrust.at";
    private const string URL_PROD = "https://rksv.fiskaltrust.at";

    private readonly HttpClient _client;

    public RestRksvClient(string cashboxId, string accessToken, bool isSandbox = true)
    {
        _client = new HttpClient { BaseAddress = new Uri(isSandbox ? URL_SANDBOX : URL_PROD) };
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", $"{cashboxId}:{accessToken}");
    }

    public async Task<byte[]> CertificateAsync() => await _client.GetByteArrayAsync("certificate");

    public async Task<string> EchoAsync(string message)
    {
        var requestContent = JsonSerializer.Serialize(new EchoRequest { Message = message });
        var response = await _client.PostAsync("echo", new StringContent(requestContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<EchoResponse>(responseContent).Message;
    }

    public async Task<byte[]> SignAsync(byte[] data)
    {
        var requestContent = JsonSerializer.Serialize(new SignRequest { DataBase64 = Convert.ToBase64String(data) });
        var response = await _client.PostAsync("sign", new StringContent(requestContent, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        return Convert.FromBase64String(JsonSerializer.Deserialize<SignResponse>(responseContent).SignedDataBase64);
    }

    public async Task<string> ZDAAsync() => await _client.GetStringAsync("zda");

    public void Dispose() => _client.Dispose();
}
