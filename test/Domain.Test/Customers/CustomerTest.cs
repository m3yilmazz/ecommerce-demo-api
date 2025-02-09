using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;

namespace Domain.Test.Customers;

[TestFixture]
public class CustomerTest
{
    private readonly Customer _customer;

    public CustomerTest()
    {
        _customer = new Customer("David", "Wilson", "5th Ave.", "12345");
    }

    #region ConstructorTests

    [Test]
    public void Constructor_ShouldPass_WhenValidValues()
    {
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";

        var customer = new Customer(name, lastName, address, postalCode);

        Assert.That(customer, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(customer.Name, Is.EqualTo(name));
            Assert.That(customer.LastName, Is.EqualTo(lastName));
            Assert.That(customer.Address, Is.EqualTo(address));
            Assert.That(customer.PostalCode, Is.EqualTo(postalCode));
        });
    }
    
    [Test]
    [TestCase("E", "Smith", "10th Ave.", "45678", "Customer name must be more than at least 1 character long.")]
    [TestCase("Emma", "S", "10th Ave.", "45678", "Customer last name must be more than at least 1 character long.")]
    [TestCase("Emma", "Smith", "1", "45678", "Customer address must be more than at least 1 character long.")]
    [TestCase("Emaa", "Smith", "10th Ave.", "4", "Customer postal code must be more than at least 1 character long.")]
    public void Constructor_ShouldThrowException_WhenInvalidValues(string name, string lastName, string address, string postalCode, string errorMessage)
    {
        var ex = Assert.Throws<BusinessRuleException>(() => new Customer(name, lastName, address, postalCode));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    #endregion ConstructorTests

    #region SetNameTests

    [Test]
    public void SetName_ShouldPass_WithValidValue()
    {
        var name = "Robert";

        _customer.SetName(name);

        Assert.That(_customer.Name, Is.EqualTo(name));
    }

    [Test]
    public void SetName_ShouldThrowException_WithNullValue()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _customer.SetName(null));
        
        Assert.That(ex.Message, Is.EqualTo("name cannot be null. (Parameter 'name')"));
    }

    [Test]
    [TestCase("", "name cannot be empty. (Parameter 'name')")]
    [TestCase(" ", "name cannot be empty or consist only of white-space characters. (Parameter 'name')")]
    public void SetName_ShouldThrowException_WithInvalidValues(string name, string errorMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => _customer.SetName(name));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    [Test]
    public void SetName_ShouldThrowException_WithInvalidValue()
    {
        var name = "A";

        var ex = Assert.Throws<BusinessRuleException>(() => _customer.SetName(name));

        Assert.That(ex.Message, Is.EqualTo("Customer name must be more than at least 1 character long."));
    }

    #endregion SetNameTests

    #region SetLastNameTests

    [Test]
    public void SetLastName_ShouldPass_WithValidValue()
    {
        var lastName = "Jackson";

        _customer.SetLastName(lastName);

        Assert.That(_customer.LastName, Is.EqualTo(lastName));
    }

    [Test]
    public void SetLastName_ShouldThrowException_WithNullValue()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _customer.SetLastName(null));

        Assert.That(ex.Message, Is.EqualTo("lastName cannot be null. (Parameter 'lastName')"));
    }

    [Test]
    [TestCase("", "lastName cannot be empty. (Parameter 'lastName')")]
    [TestCase(" ", "lastName cannot be empty or consist only of white-space characters. (Parameter 'lastName')")]
    public void SetLastName_ShouldThrowException_WithInvalidValues(string lastName, string errorMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => _customer.SetLastName(lastName));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    [Test]
    public void SetLastName_ShouldThrowException_WithInvalidValue()
    {
        var lastName = "Z";

        var ex = Assert.Throws<BusinessRuleException>(() => _customer.SetLastName(lastName));

        Assert.That(ex.Message, Is.EqualTo("Customer last name must be more than at least 1 character long."));
    }

    #endregion SetLastNameTests

    #region SetAddressTests

    [Test]
    public void SetAddress_ShouldPass_WithValidValue()
    {
        var address = "Manhattan";

        _customer.SetAddress(address);

        Assert.That(_customer.Address, Is.EqualTo(address));
    }

    [Test]
    public void SetAddress_ShouldThrowException_WithNullValue()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _customer.SetAddress(null));

        Assert.That(ex.Message, Is.EqualTo("address cannot be null. (Parameter 'address')"));
    }

    [Test]
    [TestCase("", "address cannot be empty. (Parameter 'address')")]
    [TestCase(" ", "address cannot be empty or consist only of white-space characters. (Parameter 'address')")]
    public void SetAddress_ShouldThrowException_WithInvalidValues(string address, string errorMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => _customer.SetAddress(address));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    [Test]
    public void SetAddress_ShouldThrowException_WithInvalidValue()
    {
        var address = "B";

        var ex = Assert.Throws<BusinessRuleException>(() => _customer.SetAddress(address));

        Assert.That(ex.Message, Is.EqualTo("Customer address must be more than at least 1 character long."));
    }

    #endregion SetAddressTests

    #region SetPostalCodeTests

    [Test]
    public void SetPostalCode_ShouldPass_WithValidValue()
    {
        var postalCode = "98765";

        _customer.SetPostalCode(postalCode);

        Assert.That(_customer.PostalCode, Is.EqualTo(postalCode));
    }

    [Test]
    public void SetPostalCode_ShouldThrowException_WithNullValue()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => _customer.SetPostalCode(null));

        Assert.That(ex.Message, Is.EqualTo("postalCode cannot be null. (Parameter 'postalCode')"));
    }

    [Test]
    [TestCase("", "postalCode cannot be empty. (Parameter 'postalCode')")]
    [TestCase(" ", "postalCode cannot be empty or consist only of white-space characters. (Parameter 'postalCode')")]
    public void SetPostalCode_ShouldThrowException_WithInvalidValues(string address, string errorMessage)
    {
        var ex = Assert.Throws<ArgumentException>(() => _customer.SetPostalCode(address));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));
    }

    [Test]
    public void SetPostalCode_ShouldThrowException_WithInvalidValue()
    {
        var postalCode = "P";

        var ex = Assert.Throws<BusinessRuleException>(() => _customer.SetPostalCode(postalCode));

        Assert.That(ex.Message, Is.EqualTo("Customer postal code must be more than at least 1 character long."));
    }

    #endregion SetPostalCodeTests
}