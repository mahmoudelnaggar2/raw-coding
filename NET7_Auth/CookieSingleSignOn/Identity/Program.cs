using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection()
    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect("127.0.0.1:6379"))
    .SetApplicationName("unique");

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.Cookie.Domain = ".company.local";
    });

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World! identity");
app.MapGet("/protected", () => "secret!").RequireAuthorization();
app.MapGet("/login", (HttpContext ctx) =>
{
    ctx.SignInAsync(new ClaimsPrincipal(new[]
    {
        new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            },
            CookieAuthenticationDefaults.AuthenticationScheme)
    }));

    return "ok";
});

app.Run();