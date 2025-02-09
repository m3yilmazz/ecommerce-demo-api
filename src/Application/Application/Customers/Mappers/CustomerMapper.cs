using Application.Application.Customers.Dtos;
using Core.Domain.Customers;

namespace Application.Application.Customers.Mappers;

public static class CustomerMapper
{
    public static CustomerDto Map(this Customer customer) => new(customer.Id, customer.Name, customer.LastName, customer.Address, customer.PostalCode);
}