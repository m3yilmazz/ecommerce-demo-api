namespace Application.API.Models.Orders;

/// <summary>
/// Represents a request to create a order with details.
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// The unique identifier of the customer which will have relation with the order.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The items of the order that contains.
    /// </summary>
    public List<CreateOrderItemModel> Items { get; set; }
}