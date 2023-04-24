using System.Diagnostics;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;

namespace ConsoleAppAuthentication;

public class Auth
{
    public static async Task Handler()
    {
        var sem = new SemaphoreSlim(0);
        var builder = WebApplication.CreateBuilder();
        builder.WebHost.UseUrls("https://localhost:5000");
        builder.Logging.ClearProviders();
        builder.Services.AddTransient<PersistedAccessToken>();
        builder.Services.AddDataProtection()
            .SetApplicationName("my_cli_app")
            .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "cli_encryption_key"
            )));
        builder.Services.AddAuthentication()
            .AddOAuth<OAuthOptionsWithoutSecret, OAuthHandlerWithoutSecret>("default", o =>
            {
                o.ClientId = "0848ebb2-4d99-4a27-ab79-48bbfac3f42d";
                o.AuthorizationEndpoint =
                    "https://login.microsoftonline.com/e58e43d8-c6f6-42b1-bd24-0e15a4185605/oauth2/v2.0/authorize";
                o.TokenEndpoint =
                    "https://login.microsoftonline.com/e58e43d8-c6f6-42b1-bd24-0e15a4185605/oauth2/v2.0/token";
                o.CallbackPath = "/auth-cb";
                o.Scope.Add("User.Read");
                o.Scope.Add("openid");
                o.Scope.Add("email");
                o.Scope.Add("profile");

                o.Events.OnCreatingTicket = ctx =>
                {
                    var pat = ctx.HttpContext.RequestServices.GetRequiredService<PersistedAccessToken>();
                    pat.SaveAsync(ctx.AccessToken);
                    ctx.HttpContext.Response.Redirect("/success");
                    return Task.CompletedTask;
                };
            });
        var app = builder.Build();
        app.UseAuthentication();
        app.MapGet("/", () => Results.Challenge(new(), new List<string>() { "default" }));
        app.MapGet("/success", () =>
        {
            sem.Release();
            return "success!";
        });
        app.StartAsync();
        
        Process.Start(new ProcessStartInfo()
        {
            FileName = "https://localhost:5000",
            UseShellExecute = true,
        });
        
        await sem.WaitAsync();

        await app.StopAsync();

    }
}

public class OAuthHandlerWithoutSecret : OAuthHandler<OAuthOptionsWithoutSecret>
{
    public OAuthHandlerWithoutSecret(IOptionsMonitor<OAuthOptionsWithoutSecret> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
    {
        await base.HandleRemoteAuthenticateAsync();
        return HandleRequestResult.Handle();
    }
}

public class OAuthOptionsWithoutSecret : OAuthOptions
{
    public override void Validate()
    {
        
    }
}