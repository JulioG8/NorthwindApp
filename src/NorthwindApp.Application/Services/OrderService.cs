using NorthwindApp.Application.DTOs;
using NorthwindApp.Application.Interfaces;
using NorthwindApp.Domain.Interfaces;

namespace NorthwindApp.Application.Services;

/// <summary>
/// Order service implementation
/// Follows Single Responsibility Principle (SRP) - handles only order-related business logic
/// </summary>
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    /// <summary>
    /// Gets orders for a customer, sorted by shipped date
    /// </summary>
    public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerIdAsync(string customerId)
    {
        var orders = await _orderRepository.GetOrdersByCustomerIdAsync(customerId);

        return orders
            .OrderBy(o => o.ShippedDate)
            .Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                CustomerId = o.CustomerId,
                OrderDate = o.OrderDate,
                ShippedDate = o.ShippedDate
            });
    }
}
