using Microsoft.EntityFrameworkCore;
using NorthwindApp.Domain.Entities;
using NorthwindApp.Domain.Interfaces;
using NorthwindApp.Infrastructure.Data;

namespace NorthwindApp.Infrastructure.Repositories;

/// <summary>
/// Order repository implementation using Entity Framework Core
/// Follows Repository Pattern for data access abstraction
/// </summary>
public class OrderRepository : IOrderRepository
{
    private readonly NorthwindDbContext _context;

    public OrderRepository(NorthwindDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(string customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders.FindAsync(orderId);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders.ToListAsync();
    }
}
