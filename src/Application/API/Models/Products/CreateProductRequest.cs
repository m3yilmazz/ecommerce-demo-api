namespace Application.API.Models.Products;

/// <summary>
/// Represents a request to create a product with details.
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// The name of the new product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The price of the new product.
    /// </summary>
    public double Price { get; set; }
}