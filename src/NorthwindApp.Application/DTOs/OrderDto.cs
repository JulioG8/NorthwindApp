namespace NorthwindApp.Application.DTOs;

/// <summary>
/// Data Transfer Object for Order
/// </summary>
public record OrderDto
{
    public int OrderId { get; init; }
    public string? CustomerId { get; init; }
    public DateTime? OrderDate { get; init; }
    public DateTime? ShippedDate { get; init; }
}
