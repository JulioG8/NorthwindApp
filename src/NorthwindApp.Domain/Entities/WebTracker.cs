namespace NorthwindApp.Domain.Entities;

/// <summary>
/// Represents a web tracking entry for logging all site requests
/// </summary>
public class WebTracker
{
    public int Id { get; set; }
    public string UrlRequest { get; set; } = string.Empty;
    public string SourceIp { get; set; } = string.Empty;
    public DateTime TimeOfAction { get; set; } = DateTime.UtcNow;
}
