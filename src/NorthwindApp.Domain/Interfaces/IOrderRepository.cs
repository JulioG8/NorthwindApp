using NorthwindApp.Domain.Entities;

namespace NorthwindApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Order operations
/// Follows Interface Segregation Principle (ISP) - SOLID
/// </summary>
public interface IOrderRepository
{
    Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId);
    Task<Order?> GetOrderByIdAsync(int orderId);
    Task<IEnumerable<Order>> GetAllOrdersAsync();
}
