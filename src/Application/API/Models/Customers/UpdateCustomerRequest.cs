namespace Application.API.Models.Customers;

/// <summary>
/// Represents a request to update an existing customer with details.
/// </summary>
public class UpdateCustomerRequest
{
    /// <summary>
    /// The new name of the existing customer.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The new last name of the existing customer.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// The new address of the existing customer.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The new postal code of the existing customer.
    /// </summary>
    public string PostalCode { get; set; }
}