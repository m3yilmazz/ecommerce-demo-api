using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;

namespace Domain.Test.Orders;

[TestFixture]
public class ItemTest
{
    private readonly Item _item;

    public ItemTest()
    {
        _item = new Item(Guid.NewGuid(), 100);
    }

    #region ConstructorTests

    [Test]
    public void Constructor_ShouldPass_WhenValidValues()
    {
        var productId = Guid.NewGuid();
        var quantityOfProduct = 10;

        var item = new Item(productId, quantityOfProduct);

        Assert.That(item, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(item.ProductId, Is.EqualTo(productId));
            Assert.That(item.QuantityOfProduct, Is.EqualTo(quantityOfProduct));
        });
    }
    
    [Test]
    public void Constructor_ShouldThrowException_WhenInvalidProductId()
    {
        var productId = Guid.Empty;
        var quantityOfProduct = 10;

        var ex = Assert.Throws<ArgumentException>(() => new Item(productId, quantityOfProduct));

        Assert.That(ex.Message, Is.EqualTo($"productId cannot be {Guid.Empty}. (Parameter 'productId')"));
    }
    
    [Test]
    [TestCase(0, "quantityOfProduct cannot be zero. (Parameter 'quantityOfProduct')\r\nActual value was 0.")]
    [TestCase(-10, "quantityOfProduct must be zero or greater. (Parameter 'quantityOfProduct')\r\nActual value was -10.")]
    public void Constructor_ShouldThrowException_WhenInvalidQuantityOfProduct(int quantityOfProduct, string errorMessage)
    {
        var productId = Guid.NewGuid();

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Item(productId, quantityOfProduct));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    #endregion ConstructorTests

    #region SetQuantityOfProductTests

    [Test]
    public void SetQuantityOfProduct_ShouldPass_WithValidValue()
    {
        var quantityOfProduct = 5;

        _item.SetQuantityOfProduct(quantityOfProduct);

        Assert.That(_item.QuantityOfProduct, Is.EqualTo(quantityOfProduct));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-10)]
    public void SetQuantityOfProduct_ShouldThrowException_WithDefaultValue(int quantityOfProduct)
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _item.SetQuantityOfProduct(quantityOfProduct));

        Assert.That(ex.Message, Is.EqualTo("Quantity of product must be greater than zero."));
    }

    #endregion SetQuantityOfProductTests

    #region IncreaseQuantityOfProductTests

    [Test]
    public void IncreaseQuantityOfProduct_ShouldPass_WithValidValue()
    {
        var previousQuantity = _item.QuantityOfProduct;
        var addedQuantityOfProduct = 5;

        _item.IncreaseQuantityOfProduct(addedQuantityOfProduct);

        Assert.That(_item.QuantityOfProduct, Is.EqualTo(previousQuantity + addedQuantityOfProduct));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-10)]
    public void IncreaseQuantityOfProduct_ShouldThrowException_WithDefaultValue(int quantityOfProduct)
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _item.IncreaseQuantityOfProduct(quantityOfProduct));

        Assert.That(ex.Message, Is.EqualTo("Quantity of product must be greater than zero."));
    }

    #endregion IncreaseQuantityOfProductTests

    #region DecreaseQuantityOfProductTests

    [Test]
    public void DecreaseQuantityOfProduct_ShouldPass_WithValidValue()
    {
        var previousQuantity = _item.QuantityOfProduct;
        var addedQuantityOfProduct = 5;

        _item.DecreaseQuantityOfProduct(addedQuantityOfProduct);

        Assert.That(_item.QuantityOfProduct, Is.EqualTo(previousQuantity - addedQuantityOfProduct));
    }

    [Test]
    [TestCase(0)]
    [TestCase(-10)]
    public void DecreaseQuantityOfProduct_ShouldThrowException_WithDefaultValue(int quantityOfProduct)
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _item.DecreaseQuantityOfProduct(quantityOfProduct));

        Assert.That(ex.Message, Is.EqualTo("Quantity of product must be greater than zero."));
    }
    
    [Test]
    public void DecreaseQuantityOfProduct_ShouldThrowException_WithEqualQuantityValue()
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _item.DecreaseQuantityOfProduct(_item.QuantityOfProduct));

        Assert.That(ex.Message, Is.EqualTo("Item must have at least one quantity of a product. Can not remove more than or equal quantity as current quantity of a product."));
    }

    #endregion DecreaseQuantityOfProductTests
}