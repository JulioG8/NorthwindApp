namespace NorthwindApp.Application.DTOs;

/// <summary>
/// Data Transfer Object for Customer
/// </summary>
public record CustomerDto
{
    public string CustomerId { get; init; } = string.Empty;
    public string CompanyName { get; init; } = string.Empty;
    public string? ContactName { get; init; }
    public string? Phone { get; init; }
    public string? Fax { get; init; }
    public string? Country { get; init; }
}
