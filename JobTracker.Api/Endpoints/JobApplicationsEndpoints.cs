using JobTracker.Api.Data;
using JobTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Api.Endpoints;

public static class JobApplicationsEndpoints
{
    public static IEndpointRouteBuilder MapJobApplicationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/job-applications").WithTags("JobApplications");

        group.MapGet("", async (AppDbContext db, int? skip, int? take, ApplicationStatus? status) =>
        {
            var query = db.JobApplications.AsQueryable();
            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status);
            }
            query = query.OrderByDescending(x => x.AppliedAt);
            if (skip.HasValue) query = query.Skip(skip.Value);
            if (take.HasValue) query = query.Take(Math.Clamp(take.Value, 1, 200));
            var list = await query.ToListAsync();
            return Results.Ok(list);
        });

        group.MapGet("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.JobApplications.FindAsync(id);
            return entity is not null ? Results.Ok(entity) : Results.NotFound();
        });

        group.MapPost("", async (JobApplication create, AppDbContext db) =>
        {
            db.JobApplications.Add(create);
            await db.SaveChangesAsync();
            return Results.Created($"/api/job-applications/{create.Id}", create);
        });

        group.MapPut("/{id:int}", async (int id, JobApplication update, AppDbContext db) =>
        {
            var entity = await db.JobApplications.FindAsync(id);
            if (entity is null) return Results.NotFound();
            entity.Company = update.Company;
            entity.Position = update.Position;
            entity.Url = update.Url;
            entity.AppliedAt = update.AppliedAt;
            entity.Status = update.Status;
            entity.Notes = update.Notes;
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, AppDbContext db) =>
        {
            var entity = await db.JobApplications.FindAsync(id);
            if (entity is null) return Results.NotFound();
            db.JobApplications.Remove(entity);
            await db.SaveChangesAsync();
            return Results.NoContent();
        });

        return endpoints;
    }
}


