using NorthwindApp.Domain.Entities;

namespace NorthwindApp.Domain.Interfaces;

/// <summary>
/// Repository interface for Customer operations
/// Follows Interface Segregation Principle (ISP) - SOLID
/// </summary>
public interface ICustomerRepository
{
    Task<IEnumerable<Customer>> GetCustomersByCountryAsync(string country);
    Task<Customer?> GetCustomerByIdAsync(string customerId);
    Task<IEnumerable<Customer>> GetAllCustomersAsync();
}
