using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using Dawn;

namespace Core.Domain.Customers;

public class Customer : AggregateRoot
{
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public string Address { get; private set; }
    public string PostalCode { get; private set; }

    private readonly List<Order> _orders = [];
    public IReadOnlyCollection<Order> Orders => _orders.AsReadOnly();

    /// <summary>
    /// EF Core needs empty constructor
    /// </summary>
    private Customer()
    {
        
    }

    public Customer(string name, string lastName, string address, string postalCode)
    {
        SetName(name);
        SetLastName(lastName);
        SetAddress(address);
        SetPostalCode(postalCode);
    }

    public void SetName(string name)
    {
        Guard.Argument(name, nameof(name)).NotNull().NotEmpty().NotWhiteSpace();

        if (name.Length <= 1)
            throw new BusinessRuleException("Customer name must be more than at least 1 character long.");

        Name = name;
    }

    public void SetLastName(string lastName)
    {
        Guard.Argument(lastName, nameof(lastName)).NotNull().NotEmpty().NotWhiteSpace();

        if (lastName.Length <= 1)
            throw new BusinessRuleException("Customer last name must be more than at least 1 character long.");

        LastName = lastName;
    }
    
    public void SetAddress(string address)
    {
        Guard.Argument(address, nameof(address)).NotNull().NotEmpty().NotWhiteSpace();

        if (address.Length <= 1)
            throw new BusinessRuleException("Customer address must be more than at least 1 character long.");

        Address = address;
    }
    
    public void SetPostalCode(string postalCode)
    {
        Guard.Argument(postalCode, nameof(postalCode)).NotNull().NotEmpty().NotWhiteSpace();

        if (postalCode.Length <= 1)
            throw new BusinessRuleException("Customer postal code must be more than at least 1 character long.");

        PostalCode = postalCode;
    }
}