var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/api", () => "Hello World from API!");

app.UseRouting();

app.UseEndpoints(_ => { });

app.Use((ctx, next) =>
{
    if (ctx.Request.Path.StartsWithSegments("/api"))
    {
        ctx.Response.StatusCode = 404;
        return Task.CompletedTask;
    }

    return next();
});

app.UseSpa(x =>
{
    x.UseProxyToSpaDevelopmentServer("http://127.0.0.1:5173");
});

app.Run();