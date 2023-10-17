using Microsoft.EntityFrameworkCore;

namespace HW_15_10_23.Models;

public class SiteDbContext : DbContext
{
    public SiteDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Task> Tasks { get; set; }
}
