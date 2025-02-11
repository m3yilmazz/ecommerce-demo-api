using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using Dawn;

namespace Core.Domain.Orders;

public class Order : AggregateRoot
{
    public DateTime OrderDate { get; }
    public double TotalPrice { get; private set; }

    public Guid CustomerId { get; }
    public virtual Customer Customer { get; set; }

    private readonly List<Item> _items = [];
    public IReadOnlyCollection<Item> Items => _items.AsReadOnly();

    /// <summary>
    /// EF Core needs empty constructor
    /// </summary>
    private Order()
    {

    }

    public Order(Guid customerId, List<Item> items)
    {
        Guard.Argument(customerId, nameof(customerId)).NotDefault();
        Guard.Argument(items, nameof(items)).NotNull();

        if (items.Count == 0)
            throw new BusinessRuleException("An order must contain at least one item.");

        OrderDate = DateTime.UtcNow;
        CustomerId = customerId;
        _items.AddRange(items);
    }

    public void SetTotalPrice(double totalPrice)
    {
        Guard.Argument(totalPrice, nameof(totalPrice)).NotNegative();

        TotalPrice = totalPrice;
    }

    public void IncreaseTotalPrice(double totalPrice)
    {
        Guard.Argument(totalPrice, nameof(totalPrice)).NotNegative();

        TotalPrice += totalPrice;
    }

    public void DecreaseTotalPrice(double totalPrice)
    {
        Guard.Argument(totalPrice, nameof(totalPrice)).NotNegative();

        if (totalPrice > TotalPrice)
            throw new BusinessRuleException("Decrease price must be lower than or equal to TotalPrice.");

        TotalPrice -= totalPrice;
    }

    public void AddItem(Guid productId, int quantity)
    {
        Guard.Argument(productId, nameof(productId)).NotDefault();
        Guard.Argument(quantity, nameof(quantity)).NotNegative();

        var existingItem = _items.FirstOrDefault(s => s.ProductId == productId);

        if (existingItem != null)
        {
            existingItem.IncreaseQuantityOfProduct(quantity);
            existingItem.SetUpdatedAt();
        }
        else
        {
            _items.Add(new Item(productId, quantity));
        }
    }

    public void UpdateItem(Guid productId, int quantity)
    {
        Guard.Argument(productId, nameof(productId)).NotDefault();
        Guard.Argument(quantity, nameof(quantity)).NotZero().NotNegative();

        var item = _items.FirstOrDefault(s => s.ProductId == productId) ??
            throw new BusinessRuleException("Item with the given ProductId was not found.");

        item.SetQuantityOfProduct(quantity);
        item.SetUpdatedAt();
    }

    public void RemoveItem(Guid productId)
    {
        Guard.Argument(productId, nameof(productId)).NotDefault();

        var item = _items.FirstOrDefault(s => s.ProductId == productId) ??
            throw new BusinessRuleException("Item with the given ProductId was not found.");

        var decreasePriceOfItem = item.Product.Price * item.QuantityOfProduct;

        DecreaseTotalPrice(decreasePriceOfItem);

        _items.Remove(item);
    }

    public void RemoveAllItems()
    {
        _items.Clear();
    }
}