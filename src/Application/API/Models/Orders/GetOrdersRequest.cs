using Application.Application.Models;

namespace Application.API.Models.Orders;

/// <summary>
/// Represents a request to retrieve orders with optional filters.
/// </summary>
public class GetOrdersRequest : ArrayBaseRequest
{
    /// <summary>
    /// The unique identifier of the customer to retrieve its orders. It's optional.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    /// The bool flag for sorting the orders by order date.
    /// </summary>
    public bool SortByOrderDateDescending { get; set; }

    /// <summary>
    /// The start time of order date.
    /// </summary>
    public DateTime? OrderDateStartAt { get; set; }


    /// <summary>
    /// The end time of order date.
    /// </summary>
    public DateTime? OrderDateEndAt { get; set; }
}