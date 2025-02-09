using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using Core.Domain.Products;

namespace Domain.Test.Orders;

[TestFixture]
public class OrderTest
{
    private Order _order;

    [SetUp]
    public void OneTimeSetUp()
    {
        var products = new List<Product>();
        for (int i = 0; i < 5; i++)
        {
            var product = new Product($"Product {i + 1}", i + 1);
            product.SetId(Guid.NewGuid());

            products.Add(product);
        }

        var items = new List<Item>();
        for (int i = 0; i < 5; i++)
        {
            var product = products.ElementAt(new Random().Next(0, 5));
            var item = new Item(product.Id, i + 1);
            item.SetId(Guid.NewGuid());
            item.SetProduct(product);

            items.Add(item);
        }

        _order = new Order(Guid.NewGuid(), items);

        var totalPrice = _order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        _order.SetTotalPrice(totalPrice);
    }

    [TearDown]
    public void OneTimeTearDown()
    {
        _order.RemoveAllItems();
        _order = null;
    }

    private Product ProductFactory(string name, double price)
    {
        var product = new Product(name, price);
        product.SetId(Guid.NewGuid());

        return product;
    }

    private Item ItemFactory(Product product, int quantity)
    {
        var item = new Item(product.Id, quantity);
        item.SetId(product.Id);
        item.SetProduct(product);

        return item;
    }

    #region ConstructorTests

    [Test]
    public void Constructor_ShouldPass_WhenValidValues()
    {
        var product = ProductFactory("Product 101", 101);

        var items = new List<Item>
        {
            ItemFactory(product, 10)
        };

        var order = new Order(Guid.NewGuid(), items);
        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        Assert.That(order, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(order.Items, Has.Count.EqualTo(1));
            Assert.That(order.OrderDate, Is.Not.EqualTo(default(DateTime)));
            Assert.That(order.TotalPrice, Is.GreaterThanOrEqualTo(0));
            Assert.That(order.CustomerId, Is.Not.EqualTo(Guid.Empty));
        });
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenInvalidCustomerId()
    {
        var product = ProductFactory("Product 101", 101);

        var items = new List<Item>
        {
            ItemFactory(product, 10)
        };

        var ex = Assert.Throws<ArgumentException>(() => new Order(Guid.Empty, items));

        Assert.That(ex.Message, Is.EqualTo($"customerId cannot be {Guid.Empty}. (Parameter 'customerId')"));
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenNullItems()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new Order(Guid.NewGuid(), null));

        Assert.That(ex.Message, Is.EqualTo("items cannot be null. (Parameter 'items')"));
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenInvalidItems()
    {
        var ex = Assert.Throws<BusinessRuleException>(() => new Order(Guid.NewGuid(), new List<Item>()));

        Assert.That(ex.Message, Is.EqualTo("An order must contain at least one item."));
    }

    #endregion ConstructorTests

    #region SetTotalPriceTests

    [Test]
    [TestCase(0)]
    [TestCase(123456789)]
    public void SetTotalPrice_ShouldPass_WhenValidValues(double totalPrice)
    {
        _order.SetTotalPrice(totalPrice);

        Assert.That(_order.TotalPrice, Is.EqualTo(totalPrice));
    }

    [Test]
    [TestCase(-0.1)]
    [TestCase(-123456789)]
    public void SetTotalPrice_ShouldThrowException_WhenInvalidValues(double totalPrice)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _order.SetTotalPrice(totalPrice));

        Assert.That(ex.Message, Is.EqualTo($"totalPrice must be zero or greater. (Parameter 'totalPrice')\r\nActual value was {totalPrice}."));
    }

    #endregion SetTotalPriceTests

    #region IncreaseTotalPriceTests

    [Test]
    [TestCase(0)]
    [TestCase(123456789)]
    public void IncreaseTotalPriceTests_ShouldPass_WhenValidValues(double totalPrice)
    {
        var previousTotalPrice = _order.TotalPrice;
        _order.IncreaseTotalPrice(totalPrice);

        Assert.That(_order.TotalPrice, Is.EqualTo(previousTotalPrice + totalPrice));
    }

    [Test]
    [TestCase(-0.1)]
    [TestCase(-123456789)]
    public void IncreaseTotalPriceTests_ShouldThrowException_WhenInvalidValues(double totalPrice)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _order.IncreaseTotalPrice(totalPrice));

        Assert.That(ex.Message, Is.EqualTo($"totalPrice must be zero or greater. (Parameter 'totalPrice')\r\nActual value was {totalPrice}."));
    }

    #endregion IncreaseTotalPriceTests

    #region DecreaseTotalPriceTests

    [Test]
    [TestCase(0)]
    [TestCase(1)]
    public void DecreaseTotalPriceTests_ShouldPass_WhenValidValues(double totalPrice)
    {
        var previousTotalPrice = _order.TotalPrice;
        _order.DecreaseTotalPrice(totalPrice);

        Assert.That(_order.TotalPrice, Is.EqualTo(previousTotalPrice - totalPrice));
    }

    [Test]
    [TestCase(123456789)]
    public void DecreaseTotalPriceTests_ShouldThrowException_WhenHigherTotalPrice(double totalPrice)
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _order.DecreaseTotalPrice(totalPrice));

        Assert.That(ex.Message, Is.EqualTo("Decrease price must be lower than or equal to TotalPrice."));
    }

    [Test]
    [TestCase(-0.1)]
    [TestCase(-123456789)]
    public void DecreaseTotalPriceTests_ShouldThrowException_WhenInvalidValues(double totalPrice)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _order.DecreaseTotalPrice(totalPrice));

        Assert.That(ex.Message, Is.EqualTo($"totalPrice must be zero or greater. (Parameter 'totalPrice')\r\nActual value was {totalPrice}."));
    }

    #endregion DecreaseTotalPriceTests

    #region AddItemTests

    [Test]
    public void AddItem_ShouldPass_WhenValidValues()
    {
        var product = ProductFactory("Product 101", 101);

        var item = _order.Items.FirstOrDefault(f => f.ProductId == product.Id);

        var previousQuantityOfProduct = item != null ? item.QuantityOfProduct : 0;
        var quantity = 10;
        var expectedQuantity = previousQuantityOfProduct + quantity;

        _order.AddItem(product.Id, quantity);

        Assert.Multiple(() =>
        {
            Assert.That(_order.Items.Any(c => c.ProductId == product.Id), Is.EqualTo(true));
            Assert.That(_order.Items.First(f => f.ProductId == product.Id).QuantityOfProduct, Is.EqualTo(expectedQuantity));
        });
    }
    
    [Test]
    public void AddItem_ShouldThrowException_WhenInvalidPrductId()
    {
        var ex = Assert.Throws<ArgumentException>(() => _order.AddItem(Guid.Empty, 1));

        Assert.That(ex.Message, Is.EqualTo($"productId cannot be {Guid.Empty}. (Parameter 'productId')"));
    }
    
    [Test]
    public void AddItem_ShouldThrowException_WhenInvalidQuantity()
    {
        var quantity = -1;
        
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _order.AddItem(Guid.NewGuid(), quantity));

        Assert.That(ex.Message, Is.EqualTo($"quantity must be zero or greater. (Parameter 'quantity')\r\nActual value was {quantity}."));
    }

    #endregion AddItemTests
    
    #region UpdateItemTests

    [Test]
    public void UpdateItem_ShouldPass_WhenValidValues()
    {
        var item = _order.Items.First();
        var newQuantity = 150;

        _order.UpdateItem(item.ProductId, newQuantity);

        Assert.That(_order.Items.First(f => f.ProductId == item.ProductId).QuantityOfProduct, Is.EqualTo(newQuantity));
    }

    [Test]
    public void UpdateItem_ShouldThrowException_WhenInvalidPrductId()
    {
        var ex = Assert.Throws<ArgumentException>(() => _order.UpdateItem(Guid.Empty, 1));

        Assert.That(ex.Message, Is.EqualTo($"productId cannot be {Guid.Empty}. (Parameter 'productId')"));
    }
    
    [Test]
    public void UpdateItem_ShouldThrowException_WhenInvalidQuantity()
    {
        var quantity = -1;
        
        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => _order.UpdateItem(Guid.NewGuid(), quantity));

        Assert.That(ex.Message, Is.EqualTo($"quantity must be zero or greater. (Parameter 'quantity')\r\nActual value was {quantity}."));
    }

    #endregion AddItemTests

    #region RemoveItemTests

    [Test]
    public void RemoveItem_ShouldPass_WhenValidValues()
    {
        var product101 = ProductFactory("Product 101", 101);
        var product102 = ProductFactory("Product 102", 102);

        var items = new List<Item>
        {
            ItemFactory(product101, 10),
            ItemFactory(product102, 10),
        };

        var order = new Order(Guid.NewGuid(), items);
        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var itemToBeRemoved = items.First();
        order.RemoveItem(itemToBeRemoved.ProductId);

        Assert.That(order.Items.Count(c => c.ProductId == itemToBeRemoved.ProductId), Is.EqualTo(0));
    }
    
    [Test]
    public void RemoveItem_ShouldThrowException_WhenInvalidProductId()
    {
        var ex = Assert.Throws<ArgumentException>(() => _order.RemoveItem(Guid.Empty));

        Assert.That(ex.Message, Is.EqualTo($"productId cannot be {Guid.Empty}. (Parameter 'productId')"));
    }
    
    [Test]
    public void RemoveItem_ShouldThrowException_WhenInalidValue()
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _order.RemoveItem(Guid.NewGuid()));

        Assert.That(ex.Message, Is.EqualTo("Item with the given ProductId was not found."));
    }

    #endregion RemoveItemTests

    #region RemoveAllItemsTests

    [Test]
    public void RemoveAllItems_ShouldPass_WhenValidValues()
    {
        _order.RemoveAllItems();

        Assert.That(_order.Items.Count, Is.EqualTo(0));
    }
    
    #endregion RemoveAllItemsTests
}