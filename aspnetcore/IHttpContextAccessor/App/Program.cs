using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication("test")
    .AddScheme<AuthenticationSchemeOptions, AlwaysAuthenticated>("test", o => { });

builder.Services.AddSingleton<AService>();
builder.Services.AddTransient<BusinessLogic>();
builder.Services.AddSingleton<BackgroundNotifications>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<BackgroundNotifications>());

var app = builder.Build();

app.MapGet("/", (ClaimsPrincipal cp) => cp.Claims.Select(x => KeyValuePair.Create(x.Type, x.Value)));

app.MapGet("/dummy", (AService s) =>
{
    s.SomethingSlowAsync();
    return "ok";
});

app.MapGet("/notify", async (BusinessLogic bl) =>
{
    await bl.NotifyUser();
    return "ok";
});

app.MapGet("/notify-bs", async (BackgroundNotifications bn) =>
{
    await bn.Writer.WriteAsync("test");
    return "ok";
});

app.Run();