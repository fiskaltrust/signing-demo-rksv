using RksvSign;
using System.ServiceModel;

internal class WcfRksvClient : IAsyncDisposable
{
    private const string URL_SANDBOX = "https://signing-sandbox.fiskaltrust.at/rksv";
    private const string URL_PROD = "https://signing.fiskaltrust.at/rksv";

    private readonly ATSSCDClient _client;

    public WcfRksvClient(string cashboxId, string accessToken, bool isSandbox = true)
    {
        var binding = new BasicHttpBinding
        {
            Security = new()
            {
                Mode = BasicHttpSecurityMode.Transport,
                Transport = new() { ClientCredentialType = HttpClientCredentialType.Basic }
            }
        };
        _client = new ATSSCDClient(binding, new EndpointAddress(isSandbox ? URL_SANDBOX : URL_PROD));
        _client.ClientCredentials.UserName.UserName = cashboxId;
        _client.ClientCredentials.UserName.Password = accessToken;
    }

    public async Task<byte[]> CertificateAsync() => await _client.CertificateAsync();

    public async Task<string> EchoAsync(string message) => await _client.EchoAsync(message);

    public async Task<byte[]> SignAsync(byte[] data) => await _client.SignAsync(data);

    public async Task<string> ZDAAsync() => await _client.ZDAAsync();

    public async ValueTask DisposeAsync()
    {
        if (_client != null) await _client.CloseAsync();
    }
}