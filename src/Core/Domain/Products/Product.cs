using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Dawn;

namespace Core.Domain.Products;

public class Product : AggregateRoot
{
    public string Name { get; private set; }
    public double Price { get; private set; }

    /// <summary>
    /// EF Core needs empty constructor
    /// </summary>
    private Product()
    {
        
    }

    public Product(string name, double price)
    {
        Guard.Argument(price, nameof(price)).NotNegative();

        SetName(name);
        Price = price;
    }

    public void SetName(string name)
    {
        Guard.Argument(name, nameof(name)).NotNull().NotEmpty().NotWhiteSpace();
        
        if (name.Length < 5)
            throw new BusinessRuleException("Product name must be at least 5 characters long.");

        Name = name;
    }

    public void SetPrice(double price)
    {
        if (price < 0)
            throw new BusinessRuleException("Price must be greater than or equal to zero.");

        Price = price; 
    }
}