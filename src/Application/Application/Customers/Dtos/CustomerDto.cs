namespace Application.Application.Customers.Dtos;

public class CustomerDto
{
    public Guid Id { get; }
    public string Name { get; }
    public string LastName { get; }
    public string Address { get; }
    public string PostalCode { get; }

    public CustomerDto(
        Guid id,
        string name,
        string lastName,
        string address,
        string postalCode)
    {
        Id = id;
        Name = name;
        LastName = lastName;
        Address = address;
        PostalCode = postalCode;
    }
}