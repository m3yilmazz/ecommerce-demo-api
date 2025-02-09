using Core.Domain.Customers;
using Core.Domain.Orders;
using Core.Domain.Products;

namespace Application.Test.Orders.Helpers;

public static class ObjectFactory
{
    public static Product ProductFactory(string name = "Product 1", double price = 100)
    {
        var product = new Product(name, price);
        product.SetId(Guid.NewGuid());

        return product;
    }

    public static Item ItemFactory(Product product, int quantity = 10)
    {
        var item = new Item(product.Id, quantity);
        item.SetId(product.Id);
        item.SetProduct(product);

        return item;
    }

    public static Customer CustomerFactory(string name = "Emma", string lastName = "Smith", string address = "10th Ave.", string postalCode = "45678")
    {
        var customer = new Customer(name, lastName, address, postalCode);
        customer.SetId(Guid.NewGuid());

        return customer;
    }
}