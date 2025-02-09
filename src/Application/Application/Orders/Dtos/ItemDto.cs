namespace Application.Application.Orders.Dtos;

public class ItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public double ProductPrice { get; set; }
    public int QuantityOfProduct { get; set; }

    public ItemDto(Guid id, Guid productId, string productName, double productPrice, int quantityOfProduct)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        ProductPrice = productPrice;
        QuantityOfProduct = quantityOfProduct;
    }
}