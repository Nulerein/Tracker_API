using JobTracker.Api.Data;
using JobTracker.Api.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Tests;

public class JobApplicationsServiceTests
{
    private static AppDbContext CreateInMemoryContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;
        var ctx = new AppDbContext(options);
        ctx.Database.EnsureCreated();
        return ctx;
    }

    [Fact]
    public async Task CanCreateAndReadJobApplication()
    {
        await using var ctx = CreateInMemoryContext();
        var app = new JobApplication
        {
            Company = "Acme Corp",
            Position = "C# Backend Developer",
            Status = ApplicationStatus.Applied
        };
        ctx.JobApplications.Add(app);
        await ctx.SaveChangesAsync();

        var fetched = await ctx.JobApplications.FirstOrDefaultAsync(x => x.Company == "Acme Corp");
        Assert.NotNull(fetched);
        Assert.Equal("C# Backend Developer", fetched!.Position);
    }
}


