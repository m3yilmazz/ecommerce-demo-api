namespace Application.Application.Orders.Dtos;

public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public int QuantityOfProduct { get; set; }

    public OrderItemDto(Guid productId, int quantityOfProduct)
    {
        ProductId = productId;
        QuantityOfProduct = quantityOfProduct;
    }
}