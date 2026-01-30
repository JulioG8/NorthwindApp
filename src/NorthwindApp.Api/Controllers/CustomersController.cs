using Microsoft.AspNetCore.Mvc;
using NorthwindApp.Application.DTOs;
using NorthwindApp.Application.Interfaces;

namespace NorthwindApp.Api.Controllers;

/// <summary>
/// API Controller for Customer operations
/// Replaces WCF service endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Gets customers filtered by country, sorted by contact name
    /// GET /api/customers/country/{country}
    /// </summary>
    [HttpGet("country/{country}")]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersByCountry(string country)
    {
        var customers = await _customerService.GetCustomersByCountryAsync(country);
        return Ok(customers);
    }

    /// <summary>
    /// Gets a single customer by ID
    /// GET /api/customers/{customerId}
    /// </summary>
    [HttpGet("{customerId}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(string customerId)
    {
        var customer = await _customerService.GetCustomerByIdAsync(customerId);

        if (customer == null)
            return NotFound($"Customer with ID '{customerId}' not found");

        return Ok(customer);
    }
}
