using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Angular.Data;

public class Database : IdentityDbContext<IdentityUser>
{
    public Database(DbContextOptions<Database> options) : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<WorkTask> Tasks { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
}

public class Project
{
    public int Id { get; set; }
    public List<WorkTask> Tasks { get; set; } = new();
    public List<ProjectUser> Users { get; set; } = new();
}

public class ProjectUser
{
    public int Id { get; set; }
    public string UserId { get; set; }
}

public class WorkTask
{
    public int Id { get; set; }
    public string Title { get; set; }
}