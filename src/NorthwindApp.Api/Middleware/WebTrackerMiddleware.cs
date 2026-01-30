using NorthwindApp.Domain.Interfaces;

namespace NorthwindApp.Api.Middleware;

/// <summary>
/// Middleware that logs all incoming requests to the WebTracker table
/// This ensures every action on the site is tracked as per requirement #4
/// </summary>
public class WebTrackerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<WebTrackerMiddleware> _logger;

    private static readonly string[] SkipExtensions =
    {
        ".css", ".js", ".png", ".jpg", ".gif", ".ico", ".woff", ".woff2", ".svg"
    };

    public WebTrackerMiddleware(RequestDelegate next, ILogger<WebTrackerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IWebTrackerRepository webTrackerRepository)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Only track API requests, skip static files
        if (ShouldTrackRequest(path))
        {
            try
            {
                var urlRequest = $"{context.Request.Method} {context.Request.Path}{context.Request.QueryString}";
                var sourceIp = GetClientIpAddress(context);

                await webTrackerRepository.LogRequestAsync(urlRequest, sourceIp);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging request to WebTracker");
            }
        }

        await _next(context);
    }

    private static bool ShouldTrackRequest(string path)
    {
        // Skip swagger and static files
        if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            return false;

        foreach (var ext in SkipExtensions)
        {
            if (path.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                return false;
        }

        return true;
    }

    private static string GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded IP first (when behind proxy/load balancer)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',')[0].Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}
