using NorthwindApp.Application.DTOs;

namespace NorthwindApp.Application.Interfaces;

/// <summary>
/// Service interface for Customer business operations
/// Follows Dependency Inversion Principle (DIP) - SOLID
/// </summary>
public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country);
    Task<CustomerDto?> GetCustomerByIdAsync(string customerId);
}
