namespace JobTracker.Api.Models;

public enum ApplicationStatus
{
    None = 0,
    Applied = 1,
    Interview = 2,
    Offer = 3,
    Rejected = 4
}

public class JobApplication
{
    public int Id { get; set; }

    public string Company { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string? Url { get; set; }

    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public string? Notes { get; set; }
}


