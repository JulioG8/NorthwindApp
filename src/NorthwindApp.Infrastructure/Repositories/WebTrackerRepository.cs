using NorthwindApp.Domain.Entities;
using NorthwindApp.Domain.Interfaces;
using NorthwindApp.Infrastructure.Data;

namespace NorthwindApp.Infrastructure.Repositories;

/// <summary>
/// WebTracker repository implementation
/// Handles logging of all web requests to the database
/// </summary>
public class WebTrackerRepository : IWebTrackerRepository
{
    private readonly NorthwindDbContext _context;

    public WebTrackerRepository(NorthwindDbContext context)
    {
        _context = context;
    }

    public async Task LogRequestAsync(WebTracker tracker)
    {
        _context.WebTrackers.Add(tracker);
        await _context.SaveChangesAsync();
    }

    public async Task LogRequestAsync(string urlRequest, string sourceIp)
    {
        var tracker = new WebTracker
        {
            UrlRequest = urlRequest,
            SourceIp = sourceIp,
            TimeOfAction = DateTime.UtcNow
        };

        await LogRequestAsync(tracker);
    }
}
