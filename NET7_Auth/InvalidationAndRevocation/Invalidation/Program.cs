using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

List<string> blacklist = new();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie", o =>
    {
        o.Events.OnValidatePrincipal = ctx =>
        {
            if (blacklist.Contains(ctx.Principal?.FindFirstValue("session")))
            {
                ctx.RejectPrincipal();
            }

            return Task.CompletedTask;
        };
    });

var app = builder.Build();

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                new Claim("session", Guid.NewGuid().ToString())
            },
            "cookie"
        )
    ),
    new AuthenticationProperties(),
    "cookie"
));

app.MapGet("/user", (ClaimsPrincipal user) => user.Claims.Select(x => new { x.Type, x.Value }).ToList());

app.MapGet("/blacklist", (string session) => { blacklist.Add(session); });

app.Run();