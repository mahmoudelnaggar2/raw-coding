
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var jwkString = "{\"additionalData\":{},\"alg\":null,\"crv\":null,\"d\":null,\"dp\":null,\"dq\":null,\"e\":\"AQAB\",\"k\":null,\"keyId\":null,\"keyOps\":[],\"kid\":null,\"kty\":\"RSA\",\"n\":\"t70M4BCSY8DyFTOQrD2Bo-5KpZeeHgXaEQ0AHLzbP4--KLblT2k9dCG33JtVMRnO_-2CtjAPGPWJUQwcDIihwJycRaVoPAMzArcmDxAAJxmhB2YCe0v5VkV41UK3M8p0daJIVZHIYD_1907aWQWxinzkKSi1Mh8tRp89U7sWdoq5h4y-5kAkxhqnLmVsaIHlSNfJLAH2Fpe1whcuP70sHMU5B0pTAXLqY_luDowB4FUrkEl7WEiFpFS4OdnoHSZY4EEcSFhnNhz4ZbaswW9TUmVz55KRnzMX2zn66je_ZbazfFNNxYiRdQUbkHXEgKtJF0nHt2wf6e2dafMW6rL3mw\",\"oth\":null,\"p\":null,\"q\":null,\"qi\":null,\"use\":null,\"x\":null,\"x5c\":[],\"x5t\":null,\"x5tS256\":null,\"x5u\":null,\"y\":null,\"keySize\":2048,\"hasPrivateKey\":false,\"cryptoProviderFactory\":{\"cryptoProviderCache\":{},\"customCryptoProvider\":null,\"cacheSignatureProviders\":true,\"signatureProviderObjectPoolCacheSize\":40}}";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication("jwt")
    .AddJwtBearer("jwt", o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
        };
        
        o.Events = new JwtBearerEvents()
        {
            OnMessageReceived = (ctx) =>
            {
                if (ctx.Request.Query.ContainsKey("t"))
                {
                    ctx.Token = ctx.Request.Query["t"];
                }

                return Task.CompletedTask;
            }
        };

        o.Configuration = new OpenIdConnectConfiguration()
        {
            SigningKeys =
            {
                JsonWebKey.Create(jwkString)
            },
        };

        o.MapInboundClaims = false;
    });
var app = builder.Build();

app.UseAuthentication();

app.MapGet("/", (HttpContext ctx) => ctx.User.FindFirst("sub")?.Value ?? "empty");

app.MapGet("/jwt", () =>
{
    var handler = new JsonWebTokenHandler();
    var token = handler.CreateToken(new SecurityTokenDescriptor()
    {
        Issuer = "https://localhost:5000",
        Subject = new ClaimsIdentity(new []
        {
            new Claim("sub", Guid.NewGuid().ToString()),
            new Claim("name", "Anton"),
        }),
        SigningCredentials = new SigningCredentials(JsonWebKey.Create(jwkString), SecurityAlgorithms.RsaSha256),
    });

    return token;
});

app.Run();