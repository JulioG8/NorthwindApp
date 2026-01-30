using NorthwindApp.Application.DTOs;

namespace NorthwindApp.Application.Interfaces;

/// <summary>
/// Service interface for Order business operations
/// Follows Dependency Inversion Principle (DIP) - SOLID
/// </summary>
public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(string customerId);
}
