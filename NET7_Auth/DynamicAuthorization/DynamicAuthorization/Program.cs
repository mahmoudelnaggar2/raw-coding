using System.Security.Claims;
using DynamicAuthorization;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

var permissionsList = new PermissionsList();

builder.Services.AddSingleton(permissionsList);
builder.Services.AddSingleton<PermissionsDatabase>();
builder.Services.AddSingleton<IAuthorizationHandler, DynamicAuthorizationHandler>();

builder.Services.AddAuthentication("cookie")
    .AddCookie("cookie");

builder.Services.AddAuthorization(b =>
{
    b.AddPolicy("dynamic", pb => pb
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes("cookie")
        .AddRequirements(new DynamicRequirement())
    );
});

var app = builder.Build();

app.MapGet("/", (
    ClaimsPrincipal user
) => user.Claims.Select(x => new { x.Type, x.Value }));

app.MapGet("/login", () => Results.SignIn(
    new ClaimsPrincipal(
        new ClaimsIdentity(
            new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
            },
            "cookie"
        )
    ),
    authenticationScheme: "cookie"
));

app.MapGet("/secret/one", () => "one").RequireAuthorization("dynamic").WithTags("auth:secret/one", "auth:secret");
app.MapGet("/secret/two", () => "two").RequireAuthorization("dynamic").WithTags("auth:secret/two", "auth:secret");
app.MapGet("/secret/three", () => "three").RequireAuthorization("dynamic").WithTags("auth:secret/three", "auth:secret");

app.MapGet("/permissions", (
    PermissionsList p
) => p);

app.MapGet("/promote", (
    string permission,
    ClaimsPrincipal user,
    PermissionsDatabase database
) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
    database.AddPermission(userId, permission);
});

var endpoints = app as IEndpointRouteBuilder;
var source = endpoints.DataSources.First();
foreach (var sourceEndpoint in source.Endpoints)
{
    var authTags = sourceEndpoint.Metadata.OfType<TagsAttribute>()
        .SelectMany(x => x.Tags)
        .Where(x => x.StartsWith("auth"));

    foreach (var authTag in authTags)
    {
        permissionsList.Add(authTag);
    }
}

app.Run();

public class DynamicAuthorizationHandler : AuthorizationHandler<DynamicRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        DynamicRequirement requirement
    )
    {
        if (context.Resource is not HttpContext httpCtx)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var endpoint = httpCtx.GetEndpoint();
        var authTags = endpoint.Metadata.OfType<TagsAttribute>()
            .SelectMany(x => x.Tags)
            .Where(x => x.StartsWith("auth"))
            .Select(x => x.Split(':').Last());

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        using var scope = httpCtx.RequestServices.CreateScope();

        var database = scope.ServiceProvider.GetRequiredService<PermissionsDatabase>();
        foreach (var authTag in authTags)
        {
            if (database.HasPermission(userId, authTag))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }
        
        context.Fail();
        return Task.CompletedTask;

    }
}

public class DynamicRequirement : IAuthorizationRequirement {}

public class PermissionsList : HashSet<string>
{
}