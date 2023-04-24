using System.Security.Claims;

namespace Angular.Endpoints;

public class UserEndpoint
{
    public static Dictionary<string, string> Handler(ClaimsPrincipal user) => 
        user.Claims.ToDictionary(x => x.Type, x => x.Value);
}