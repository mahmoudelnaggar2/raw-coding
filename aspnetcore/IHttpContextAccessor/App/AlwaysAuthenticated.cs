using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class AlwaysAuthenticated : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public AlwaysAuthenticated(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock
    ) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync() =>
        Task.FromResult(
            AuthenticateResult.Success(
                new AuthenticationTicket(
                    new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("user_id", "test"),
                                new Claim("email", "test@test.com"),
                            },
                            "test"
                        )
                    ),
                    "test"
                )
            )
        );
}