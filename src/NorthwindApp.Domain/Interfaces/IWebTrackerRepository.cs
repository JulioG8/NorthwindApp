using NorthwindApp.Domain.Entities;

namespace NorthwindApp.Domain.Interfaces;

/// <summary>
/// Repository interface for WebTracker operations
/// Follows Interface Segregation Principle (ISP) - SOLID
/// </summary>
public interface IWebTrackerRepository
{
    Task LogRequestAsync(WebTracker tracker);
    Task LogRequestAsync(string urlRequest, string sourceIp);
}
