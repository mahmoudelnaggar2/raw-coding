using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("def")
    .AddCookie("def");

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.Use((ctx, next) =>
{
    if (ctx.User.Identity.IsAuthenticated)
    {
        if (!ctx.Request.Headers.Cookie.Any(x => x.Contains("user-info", System.StringComparison.CurrentCulture)))
        {
            var user = new { username = "anton" };
            var userJson = JsonSerializer.Serialize(user);
            var userBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(userJson));
            ctx.Response.Cookies.Append("user-info-payload", userBase64);
            ctx.Response.Cookies.Append("user-info", "1");
        }
    }

    return next();
});

app.UseEndpoints(_ => { });

app.UseSpa(x => x.UseProxyToSpaDevelopmentServer("http://localhost:3000"));

app.MapGet("/api/test", () => "secret").RequireAuthorization();

app.MapPost("/api/login", async ctx =>
{
    await ctx.SignInAsync("def", new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                },
                "def"
            )
        ),
        new AuthenticationProperties()
        {
            IsPersistent = true
        });
});

app.Run();