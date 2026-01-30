using Microsoft.EntityFrameworkCore;
using NorthwindApp.Domain.Entities;
using NorthwindApp.Domain.Interfaces;
using NorthwindApp.Infrastructure.Data;

namespace NorthwindApp.Infrastructure.Repositories;

/// <summary>
/// Customer repository implementation using Entity Framework Core
/// Follows Repository Pattern for data access abstraction
/// </summary>
public class CustomerRepository : ICustomerRepository
{
    private readonly NorthwindDbContext _context;

    public CustomerRepository(NorthwindDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country)
    {
        return await _context.Customers
            .Where(c => c.Country != null && c.Country.ToLower() == country.ToLower())
            .ToListAsync();
    }

    public async Task<Customer?> GetCustomerByIdAsync(string customerId)
    {
        return await _context.Customers.FindAsync(customerId);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }
}
