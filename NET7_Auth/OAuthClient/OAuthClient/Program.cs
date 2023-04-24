using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie")
    .AddFacebook()
    .AddOAuth("github", o =>
    {
        o.SignInScheme = "cookie";

        o.ClientId = "x";
        o.ClientSecret = "x";

        o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
        o.TokenEndpoint = "https://github.com/login/oauth/access_token";
        o.CallbackPath = "/oauth/github-cb";
        o.SaveTokens = true;
        o.UserInformationEndpoint = "https://api.github.com/user";

        o.ClaimActions.MapJsonKey("sub", "id");
        o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
        
        o.Events.OnCreatingTicket = async ctx =>
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, ctx.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", ctx.AccessToken);
            using var result = await ctx.Backchannel.SendAsync(request);
            var user = await result.Content.ReadFromJsonAsync<JsonElement>();
            ctx.RunClaimActions(user);
        };
    });

var app = builder.Build();

app.UseAuthentication();

app.MapGet("/", (
    HttpContext ctx
) =>
{
    ctx.GetTokenAsync("access_token");
    return ctx.User.Claims.Select(x => new { x.Type, x.Value }).ToList();
});

app.MapGet("/login", () =>
{
    return Results.Challenge(
        new AuthenticationProperties()
        {
            RedirectUri = "https://localhost:5005/"
        },
        authenticationSchemes: new List<string>() { "github" }
    );
});

app.Run();