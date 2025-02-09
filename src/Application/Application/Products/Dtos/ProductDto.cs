namespace Application.Application.Products.Dtos;

public class ProductDto
{
    public Guid Id { get; }
    public string Name { get; }
    public double Price { get; }

    public ProductDto(Guid id, string name, double price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}