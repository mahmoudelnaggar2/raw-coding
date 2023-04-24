using System.Net.Http.Headers;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using OAuthCorrect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie", o =>
    {
        o.LoginPath = "/login";

        var del = o.Events.OnRedirectToAccessDenied;
        o.Events.OnRedirectToAccessDenied = ctx =>
        {
            if (ctx.Request.Path.StartsWithSegments("/yt"))
            {
                return ctx.HttpContext.ChallengeAsync("youtube");
            }

            return del(ctx);
        };
    })
    .AddOAuth("youtube", o =>
    {
        o.SignInScheme = "cookie";
        o.ClientId = Secrets.ClientId;
        o.ClientSecret = Secrets.ClientSecret;
        o.SaveTokens = false;

        o.Scope.Clear();
        o.Scope.Add("https://www.googleapis.com/auth/youtube.readonly");

        o.AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        o.TokenEndpoint = "https://oauth2.googleapis.com/token";
        o.CallbackPath = "/oauth/yt-cb";

        o.Events.OnCreatingTicket = async ctx =>
        {
            var db = ctx.HttpContext.RequestServices.GetRequiredService<Database>();
            var authenticationHandlerProvider =
                ctx.HttpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            var handler = await authenticationHandlerProvider.GetHandlerAsync(ctx.HttpContext, "cookie");
            var authResult = await handler.AuthenticateAsync();
            if (!authResult.Succeeded)
            {
                ctx.Fail("failed authentication");
                return;
            }

            var cp = authResult.Principal;
            var userId = cp.FindFirstValue("user_id");
            db[userId] = ctx.AccessToken;

            ctx.Principal = cp.Clone();
            var identity = ctx.Principal.Identities.First(x => x.AuthenticationType == "cookie");
            identity.AddClaim(new Claim("yt-token", "y"));
        };
    });

builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("youtube-enabled", pb =>
    {
        pb.AddAuthenticationSchemes("cookie")
            .RequireClaim("yt-token", "y")
            .RequireAuthenticatedUser();
    });
});

builder.Services.AddSingleton<Database>()
    .AddTransient<IClaimsTransformation, YtTokenClaimsTransformation>();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            new[] { new Claim("user_id", Guid.NewGuid().ToString()) },
            "cookie"
        )
    ),
    authenticationScheme: "cookie"
));

app.MapGet("/yt/info", async (
    IHttpClientFactory clientFactory,
    HttpContext ctx
) =>
{
    var accessToken = ctx.User.FindFirstValue("yt-access_token");
    var client = clientFactory.CreateClient();

    using var req = new HttpRequestMessage(HttpMethod.Get,
        "https://www.googleapis.com/youtube/v3/channels?part=snippet&mine=true");
    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    using var response = await client.SendAsync(req);
    return await response.Content.ReadAsStringAsync();
}).RequireAuthorization("youtube-enabled");

app.Run();

public class Database : Dictionary<string, string>
{
}

public class YtTokenClaimsTransformation : IClaimsTransformation
{
    private readonly Database _db;

    public YtTokenClaimsTransformation(
        Database db
    )
    {
        _db = db;
    }

    public Task<ClaimsPrincipal> TransformAsync(
        ClaimsPrincipal principal
    )
    {
        var userId = principal.FindFirstValue("user_id");
        if (!_db.ContainsKey(userId))
        {
            return Task.FromResult(principal);
        }

        var cp = principal.Clone();
        var accessToken = _db[userId];
        
        var identity = cp.Identities.First(x => x.AuthenticationType == "cookie");
        identity.AddClaim(new Claim("yt-access_token", accessToken));
        
        return Task.FromResult(cp);
    }
}