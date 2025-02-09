using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;
using Dawn;

namespace Core.Domain.Orders;

public class Item : Entity
{
    public int QuantityOfProduct { get; private set; }

    public Guid ProductId { get; }
    public virtual Product Product { get; private set; }

    /// <summary>
    /// EF Core needs empty constructor
    /// </summary>
    private Item()
    {

    }

    public Item(Guid productId, int quantityOfProduct)
    {
        Guard.Argument(productId, nameof(productId)).NotDefault();
        Guard.Argument(quantityOfProduct, nameof(quantityOfProduct)).NotZero().NotNegative();

        ProductId = productId;
        QuantityOfProduct = quantityOfProduct;
    }

    public void SetQuantityOfProduct(int quantityOfProduct)
    {
        if (quantityOfProduct <= 0)
            throw new BusinessRuleException("Quantity of product must be greater than zero.");

        QuantityOfProduct = quantityOfProduct;
    }

    public void IncreaseQuantityOfProduct(int quantityOfProduct)
    {
        if (quantityOfProduct <= 0)
            throw new BusinessRuleException("Quantity of product must be greater than zero.");

        QuantityOfProduct += quantityOfProduct;
    }

    public void DecreaseQuantityOfProduct(int quantityOfProduct)
    {
        if (quantityOfProduct <= 0)
            throw new BusinessRuleException("Quantity of product must be greater than zero.");
        
        if (quantityOfProduct >= QuantityOfProduct)
            throw new BusinessRuleException("Item must have at least one quantity of a product. Can not remove more than or equal quantity as current quantity of a product.");

        QuantityOfProduct -= quantityOfProduct;
    }

    public void SetProduct(Product product)
    {
        Guard.Argument(product, nameof(product)).NotNull();

        Product = product;
    }
}