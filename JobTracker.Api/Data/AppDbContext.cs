using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.Property(x => x.Company).IsRequired();
            entity.Property(x => x.Position).IsRequired();
            entity.Property(x => x.Status).HasConversion<int>();
            entity.HasIndex(x => new { x.Company, x.Position });
        });
    }
}


