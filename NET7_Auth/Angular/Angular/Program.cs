using Angular.Data;
using Angular.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Database>(c => c.UseNpgsql(
    connectionString: builder.Configuration.GetConnectionString("Default")
));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        if (builder.Environment.IsDevelopment())
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
        }
    })
    .AddEntityFrameworkStores<Database>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("manager", pb => pb
        .RequireClaim("level", "manager", "admin")
    );
    options.AddPolicy("admin", pb => pb
        .RequireClaim("level", "admin")
    );
});

var app = builder.BuildWithSpa();

var apiEndpoints = app.MapGroup("/api");

apiEndpoints.MapGet("/user", UserEndpoint.Handler);
apiEndpoints.MapPost("/login", LoginEndpoint.Handler);
apiEndpoints.MapPost("/register", RegisterEndpoint.Handler);
apiEndpoints.MapGet("/logout", LogoutEndpoint.Handler).RequireAuthorization();

apiEndpoints.MapGet("/projects", ProjectEndpoints.List).RequireAuthorization();
apiEndpoints.MapGet("/projects/{id:int}", ProjectEndpoints.Get).RequireAuthorization();
apiEndpoints.MapPost("/projects/{id:int}/add-user/{userId}", ProjectEndpoints.AddUserToProject).RequireAuthorization("manager");
apiEndpoints.MapPost("/promote/{userId:guid}", PromoteEndpoint.PromoteUser).RequireAuthorization("admin");


app.Run();