namespace Application.API.Models.Orders;

/// <summary>
/// Represents a request to add an item with details to an existing order.
/// </summary>
public class AddOrderItemRequest
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// The quantity of the product.
    /// </summary>
    public int QuantityOfProduct { get; set; }
}