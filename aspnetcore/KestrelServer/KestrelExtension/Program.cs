using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Sockets;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

var shutDown = new TaskCompletionSource();
var options = new KestrelServerOptions();
options.ListenAnyIP(5001);

var server = new KestrelServer(
    Options.Create(options),
    new SocketTransportFactory(
        Options.Create(new SocketTransportOptions()),
        new NullLoggerFactory()
    ),
    new NullLoggerFactory()
);

await server.StartAsync(new HttpApp(), CancellationToken.None);
await shutDown.Task;

public class HttpApp : IHttpApplication<HttpApp.Context>
{
    public class Context
    {
        public HttpContext HttpContext { get; set; }   
    }

    public Context CreateContext(IFeatureCollection contextFeatures)
    {
        return new Context()
        {
            HttpContext = new DefaultHttpContext(contextFeatures),
        };
    }

    public async Task ProcessRequestAsync(Context context)
    {
        var req = context.HttpContext.Request;
        var res = context.HttpContext.Response;
        if (req.Path.Equals("/"))
        {
            res.StatusCode = 200;
            await using var writer = new StreamWriter(res.Body);
            await writer.WriteAsync("Hello World");
        }
        else
        {
            res.StatusCode = 404;
        }
    }

    public void DisposeContext(
        Context context,
        Exception? exception
    )
    {
        
    }
}