using Microsoft.AspNetCore.Mvc;
using NorthwindApp.Application.DTOs;
using NorthwindApp.Application.Interfaces;

namespace NorthwindApp.Api.Controllers;

/// <summary>
/// API Controller for Order operations
/// Replaces WCF service endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets orders for a customer, sorted by shipped date
    /// GET /api/orders/customer/{customerId}
    /// </summary>
    [HttpGet("customer/{customerId}")]
    public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersByCustomerId(string customerId)
    {
        var orders = await _orderService.GetOrdersByCustomerIdAsync(customerId);
        return Ok(orders);
    }
}
