using Microsoft.AspNetCore.Http;

namespace EndpointPDK;

public interface IPluginEndpoint
{
    Task Execute(HttpContext ctx);
}

public class PathAttribute : Attribute
{
    public PathAttribute(
        string method,
        string path
    )
    {
        Method = method;
        Path = path;
    }

    public string Method { get; }
    public string Path { get; }
}