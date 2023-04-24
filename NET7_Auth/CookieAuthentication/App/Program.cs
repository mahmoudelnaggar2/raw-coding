using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("default")
    .AddCookie("default", o =>
    {
        o.Cookie.Name = "mycookie";
        // o.Cookie.Domain = "";
        // o.Cookie.Path = "/test";
        // o.Cookie.HttpOnly = false;
        // o.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        // o.Cookie.SameSite = SameSiteMode.Lax;

        // o.ExpireTimeSpan = TimeSpan.FromSeconds(10);
        o.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(builder =>
{
    builder.AddPolicy("mypolicy", pb => pb
        .RequireAuthenticatedUser()
        .RequireClaim("doesntexist", "nonse")
    );
});

builder.Services.AddControllers();

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/test", () => "Hello World!").RequireAuthorization("mypolicy");

app.MapGet("/test22", async (HttpContext ctx) =>
{
    await ctx.ChallengeAsync("default",
        new AuthenticationProperties()
        {
            RedirectUri = "/anything-that-we-want"
        });
    return "ok";
});

app.MapPost("/login", async (HttpContext ctx) =>
{
    await ctx.SignInAsync("default", new ClaimsPrincipal(
            new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                },
                "default"
            )
        ),
        new AuthenticationProperties()
        {
            IsPersistent = true
        });
    return "ok";
});

app.MapGet("/signout", async (HttpContext ctx) =>
{
    await ctx.SignOutAsync("default",
        new AuthenticationProperties()
        {
            IsPersistent = true
        });
    return "ok";
});

app.MapDefaultControllerRoute();

app.Run();