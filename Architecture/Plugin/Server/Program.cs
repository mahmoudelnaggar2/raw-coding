using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using EndpointPDK;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseMiddleware<PluginMiddleware>();

app.MapGet("/", () => "Hello World!");

app.Run();

public class PluginMiddleware
{
    private readonly RequestDelegate _next;

    public PluginMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var alcRef = await Process(context);

        if (!context.Response.HasStarted)
        {
            await _next(context);
        }

        for (int i = 0; i < 10 && alcRef.IsAlive; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Console.WriteLine($"Unloading Attempt: {i}");
        }
        
        Console.WriteLine($"Unloading Successful: {!alcRef.IsAlive}");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static async Task<WeakReference> Process(HttpContext ctx)
    {
        var path =
            "/Users/toshik/ws/patreon-supporters/Architecture/Plugin/TestEndpoint/bin/Debug/net7.0/TestEndpoint.dll";

        var loadContext = new AssemblyLoadContext(path, isCollectible: true);
        try
        {
            var assembly = loadContext.LoadFromAssemblyPath(path);

            var endpointType = assembly.GetType("TestEndpoint.AnEndpoint");
            var pathInfo = endpointType?.GetCustomAttribute<PathAttribute>();

            if (
                pathInfo != null
                && pathInfo.Method.Equals(ctx.Request.Method, StringComparison.OrdinalIgnoreCase)
                && pathInfo.Path.Equals(ctx.Request.Path, StringComparison.OrdinalIgnoreCase)
            )
            {
                var endpoint = Activator.CreateInstance(endpointType) as IPluginEndpoint;
                await endpoint.Execute(ctx);
            }
        }
        finally
        {
            loadContext.Unload();
        }

        return new WeakReference(loadContext);
    }
}