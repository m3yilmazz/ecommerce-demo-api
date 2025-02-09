namespace Application.API.Models.Customers;

/// <summary>
/// Represents a request to create a customer with details.
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>
    /// The name of the new customer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The last name of the new customer.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The address of the new customer.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The postal code of the new customer.
    /// </summary>
    public string PostalCode { get; set; }
}