using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;

namespace Domain.Test.Products;

[TestFixture]
public class ProductTest
{
    private readonly Product _product;

    public ProductTest()
    {
        _product = new Product("Computer", 1100.5);
    }

    #region ConstructorTests

    [Test]
    public void Constructor_ShouldPass_WhenValidValues()
    {
        var name = "Smart Phone";
        var price = 2000;

        var product = new Product(name, price);

        Assert.That(product, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(product.Name, Is.EqualTo(name));
            Assert.That(product.Price, Is.EqualTo(price));
        });
    }

    [Test]
    public void Constructor_ShouldThrowException_WhenInvalidName()
    {
        var name = "Car";
        var price = 2000;

        var ex = Assert.Throws<BusinessRuleException>(() => new Product(name, price));

        Assert.That(ex.Message, Is.EqualTo("Product name must be at least 5 characters long."));
    }
    
    [Test]
    public void Constructor_ShouldThrowException_WhenInvalidPrice()
    {
        var name = "Tablet";
        var price = -150.75;

        var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new Product(name, price));

        Assert.That(ex.Message, Is.EqualTo($"price must be zero or greater. (Parameter 'price')\r\nActual value was {price}."));
    }

    #endregion ConstructorTests

    #region SetNameTests

    [Test]
    public void SetName_ShouldPass_WithValidValue()
    {
        var name = "Smart Phone";
        _product.SetName(name);

        Assert.That(_product.Name, Is.EqualTo(name));
    }

    [Test]
    public void SetName_ShouldThrowException_WithNullValue()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _product.SetName(null));

        Assert.That(ex.Message, Is.EqualTo("name cannot be null. (Parameter 'name')"));
    }

    [Test]
    [TestCase("", "name cannot be empty. (Parameter 'name')")]
    [TestCase(" ", "name cannot be empty or consist only of white-space characters. (Parameter 'name')")]
    public void SetName_ShouldThrowException_WithInvalidValues(string name, string errorMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => _product.SetName(name));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    [Test]
    public void SetName_ShouldThrowException_WithValidValue()
    {
        var name = "Car";

        var ex = Assert.Throws<BusinessRuleException>(() => _product.SetName(name));

        Assert.That(ex.Message, Is.EqualTo("Product name must be at least 5 characters long."));
    }

    #endregion SetNameTests

    #region SetPriceTests

    [Test]
    [TestCase(3)]
    [TestCase(123456789)]
    public void SetPrice_ShouldPass_WithValidValue(double price)
    {
        _product.SetPrice(price);

        Assert.That(_product.Price, Is.EqualTo(price));
    }

    [Test]
    [TestCase(-1.1)]
    [TestCase(-123.987)]
    public void SetPrice_ShouldThrowException_WithNegativeValue(double price)
    {
        var ex = Assert.Throws<BusinessRuleException>(() => _product.SetPrice(price));

        Assert.That(ex.Message, Is.EqualTo("Price must be greater than or equal to zero."));
    }

    #endregion SetPriceTests
}