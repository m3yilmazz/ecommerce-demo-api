namespace Application.API.Models.Orders;

/// <summary>
/// Represents a model to create an item with details.
/// </summary>
public class CreateOrderItemModel
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