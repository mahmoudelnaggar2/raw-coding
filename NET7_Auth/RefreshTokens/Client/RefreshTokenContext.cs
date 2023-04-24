using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Client;

public class RefreshTokenContext
{
    private readonly HttpClient _http;

    public RefreshTokenContext(IHttpClientFactory clientFactory)
    {
        _http = clientFactory.CreateClient();
    }

    public async Task<OAuthTokenResponse> RefreshTokenAsync(TokenInfo info, CancellationToken ct)
    {
        var tokenRequestParameters = new Dictionary<string, string>()
        {
            { "client_id", PatreonOAuthConfig.ClientId },
            { "client_secret", PatreonOAuthConfig.ClientSecret },
            { "grant_type", "refresh_token" },
            { "refresh_token", info.RefreshToken },
        };
        
        var requestContent = new FormUrlEncodedContent(tokenRequestParameters);
        var requestMessage = new HttpRequestMessage(HttpMethod.Post, PatreonOAuthConfig.TokenEndpoint);
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        requestMessage.Content = requestContent;
        requestMessage.Version = _http.DefaultRequestVersion;
        var response = await _http.SendAsync(requestMessage, ct);
        var body = await response.Content.ReadAsStringAsync(ct);

        return OAuthTokenResponse.Success(JsonDocument.Parse(body));
    }
}