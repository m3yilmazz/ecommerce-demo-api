using Application.Application.Models;

namespace Application.API.Models.Products;

/// <summary>
/// Represents a request to retrieve products with optional filters.
/// </summary>
public class GetProductsRequest : ArrayBaseRequest
{
    /// <summary>
    /// The name of the customer.
    /// </summary>
    public string Name { get; set; }
}