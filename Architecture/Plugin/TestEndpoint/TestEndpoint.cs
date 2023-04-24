using System.Text.Json;
using System.Threading.Tasks;
using EndpointPDK;
using Microsoft.AspNetCore.Http;

namespace TestEndpoint;

[Path("get", "/plug/test")]
public class AnEndpoint : IPluginEndpoint
{
    public async Task Execute(HttpContext ctx)
    {
        await ctx.Response.WriteAsJsonAsync(new { Message = "yo!a sdf asdf asdf" });
    }
}