using NorthwindApp.Application.DTOs;
using NorthwindApp.Application.Interfaces;
using NorthwindApp.Domain.Interfaces;

namespace NorthwindApp.Application.Services;

/// <summary>
/// Customer service implementation
/// Follows Single Responsibility Principle (SRP) - handles only customer-related business logic
/// Follows Open/Closed Principle (OCP) - can be extended without modification
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Gets customers filtered by country, sorted by contact name
    /// </summary>
    public async Task<IEnumerable<CustomerDto>> GetCustomersByCountryAsync(string country)
    {
        var customers = await _customerRepository.GetCustomersByCountryAsync(country);

        return customers
            .OrderBy(c => c.ContactName)
            .Select(c => new CustomerDto
            {
                CustomerId = c.CustomerId,
                CompanyName = c.CompanyName,
                ContactName = c.ContactName,
                Phone = c.Phone,
                Fax = c.Fax,
                Country = c.Country
            });
    }

    /// <summary>
    /// Gets a single customer by ID
    /// </summary>
    public async Task<CustomerDto?> GetCustomerByIdAsync(string customerId)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(customerId);

        if (customer == null)
            return null;

        return new CustomerDto
        {
            CustomerId = customer.CustomerId,
            CompanyName = customer.CompanyName,
            ContactName = customer.ContactName,
            Phone = customer.Phone,
            Fax = customer.Fax,
            Country = customer.Country
        };
    }
}
