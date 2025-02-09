namespace Application.API.Models.Products;

/// <summary>
/// Represents a request to update an existing product with details.
/// </summary>
public class UpdateProductRequest
{
    /// <summary>
    /// The new name of the existing product.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The new price of the existing product.
    /// </summary>
    public double Price { get; set; }
}