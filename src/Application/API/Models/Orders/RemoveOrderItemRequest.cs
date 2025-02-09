namespace Application.API.Models.Orders;

/// <summary>
/// Represents a request to remove an item with details from an existing order.
/// </summary>
public class RemoveOrderItemRequest
{
    /// <summary>
    /// The unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; set; }
}