using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Angular.Endpoints;

public class PromoteEndpoint
{
    public static async Task<IResult> PromoteUser(
        Guid userId,
        UserManager<IdentityUser> userManager
    )
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        await userManager.AddClaimAsync(user, new Claim("level", "manager"));
        return Results.Ok();
    }
}