using System.Security.Claims;
using Angular.Data;
using Microsoft.AspNetCore.Identity;

public static class BuildExtension
{
    public static WebApplication BuildWithSpa(this WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(_ => { });

        app.Use((
            ctx,
            next
        ) =>
        {
            if (ctx.Request.Path.StartsWithSegments("/api"))
            {
                ctx.Response.StatusCode = 404;
                return Task.CompletedTask;
            }

            return next();
        });

        app.UseSpa(x => { x.UseProxyToSpaDevelopmentServer("http://127.0.0.1:4200"); });

        using var scope = app.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        IdentityUser admin = null;
        if (!userManager.Users.Any(x => x.UserName == "admin"))
        {
            admin = new IdentityUser() { UserName = "admin" };
            userManager.CreateAsync(admin, "password").GetAwaiter().GetResult();
            userManager.AddClaimAsync(admin, new Claim("level", "admin")).GetAwaiter().GetResult();
        }

        var database = scope.ServiceProvider.GetRequiredService<Database>();
        if (!database.Projects.Any())
        {
            database.Projects.Add(new Project()
            {
                Tasks = new List<WorkTask>()
                {
                    new WorkTask() { Title = "Clean Dog" },
                    new WorkTask() { Title = "Buy Food" },
                },
                Users = new List<ProjectUser>() { new ProjectUser() { UserId = admin.Id } }
            });

            database.Projects.Add(new Project()
            {
                Tasks = new List<WorkTask>()
                {
                    new WorkTask() { Title = "Refactor Authentication" },
                    new WorkTask() { Title = "Inject Dependency" },
                },
                Users = new List<ProjectUser>() { new ProjectUser() { UserId = admin.Id } }
            });

            database.SaveChanges();
        }

        return app;
    }
}