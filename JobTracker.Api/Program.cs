
using JobTracker.Api.Data;
using JobTracker.Api.Endpoints;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // EF Core with PostgreSQL
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

        var app = builder.Build();

        // Ensure database exists
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();

        // Map domain endpoints
        app.MapJobApplicationEndpoints();

        app.Run();
    }
}
