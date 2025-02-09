using Application.Application.Models;

namespace Application.API.Models.Customers;

/// <summary>
/// Represents a request to retrieve customers with optional filters.
/// </summary>
public class GetCustomersRequest : ArrayBaseRequest
{
    /// <summary>
    /// The name of the customer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The last name of the customer.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The address of the customer.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The postal codee of the customer.
    /// </summary>
    public string PostalCode { get; set; }
}