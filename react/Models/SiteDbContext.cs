using Microsoft.EntityFrameworkCore;

namespace react.Models;

public class SiteDbContext : DbContext
{
    public SiteDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }
}
