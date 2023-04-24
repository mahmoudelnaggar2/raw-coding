namespace FormBuilder;

public static class SpaExtension
{
    public static WebApplication Setup(this WebApplicationBuilder builder)
    {
        var app = builder.Build();
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
        
        return app;
    }
}