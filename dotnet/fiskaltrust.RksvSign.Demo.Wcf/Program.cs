const string CASHBOX_ID = "<your-cashbox-id>";
const string ACCESS_TOKEN = "<your-cashbox-access-token>";

await using var rksvClient = new WcfRksvClient(CASHBOX_ID, ACCESS_TOKEN);

// Test the communication
var echoResponse = await rksvClient.EchoAsync("Hi there!");

// Get ZDA and certificate
var zda = await rksvClient.ZDAAsync();
var certificate = await rksvClient.CertificateAsync();

// Sign some data
var signedData = await rksvClient.SignAsync(new byte[] { 1, 2, 3, 4, 5 });