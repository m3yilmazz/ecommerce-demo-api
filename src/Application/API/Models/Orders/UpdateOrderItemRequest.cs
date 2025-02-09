namespace Application.API.Models.Orders;

/// <summary>
/// Represents a request to update an item with details of an existing order.
/// </summary>
public class UpdateOrderItemRequest
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