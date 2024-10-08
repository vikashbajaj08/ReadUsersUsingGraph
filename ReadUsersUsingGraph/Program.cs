using System.Net.Http.Headers;
using System.Text.Json.Serialization;

var client = new HttpClient();
var tenantId = "a33f70e2-88f3-417c-8352-1e309f0f2232";
var clientId = "50c5daf5-0b44-4cae-8bcc-7879af94a7c6";
var clientSecret = "JIb8Q~~8VqcE9YWNLxhWX1o1nEC8ugX2fF6YLbHP";

var token = await GetTokenAsync(tenantId, clientId, clientSecret);

client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

var response = await client.GetAsync("https://graph.microsoft.com/v1.0/users");
var content = await response.Content.ReadAsStringAsync();

Console.WriteLine(content);
static async Task<string> GetTokenAsync(string tenantId,string clientId, string clientSecret)
{
    try
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token");

        var content = new FormUrlEncodedContent(new[]
        {
       new KeyValuePair<string,string>("client_id",clientId),
       new KeyValuePair<string,string>("client_secret",clientSecret),
       new KeyValuePair<string,string>("scope","https://graph.microsoft.com/.default"),
       new KeyValuePair<string,string>("grant_type","client_credentials")
    });

        request.Content = content;

        var response = await client.SendAsync(request);

        var responseString = await response.Content.ReadAsStringAsync();

        dynamic tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseString);

        return tokenResponse.access_token;
    }
    catch (Exception ex)
    {

        throw;
    }
    
}