using System.Security.Claims;
using Angular.Data;
using Microsoft.EntityFrameworkCore;

namespace Angular.Endpoints;

public class ProjectEndpoints
{
    public static async Task<List<Project>> List(
        ClaimsPrincipal user,
        Database db
    )
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return await db.Projects
            .Include(x => x.Tasks)
            .Where(x => x.Users.Any(x => x.UserId == userId))
            .ToListAsync();
    }
    
    public static async Task<Project> Get(
        int id,
        ClaimsPrincipal user,
        Database db
    )
    {
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return await db.Projects
            .Include(x => x.Tasks)
            .Where(x => x.Users.Any(x => x.UserId == userId))
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public static async Task<IResult> AddUserToProject(
        int id,
        string userId,
        Database db
    )
    {
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == id);
        project.Users.Add(new ProjectUser(){ UserId = userId});
        await db.SaveChangesAsync();
        
        return Results.Ok();
    }
}