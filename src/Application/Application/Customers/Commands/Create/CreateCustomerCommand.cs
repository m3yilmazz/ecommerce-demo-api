using Application.Application.Base.Commands;
using Application.Application.Customers.Dtos;
using Application.Application.Models;
using MediatR;

namespace Application.Application.Customers.Commands.Create;

public class CreateCustomerCommand : IRequest<ObjectBaseResponse<CustomerDto>>
{
    public string Name { get; }
    public string LastName { get; }
    public string Address { get; }
    public string PostalCode { get; }

    public CreateCustomerCommand(string name, string lastName, string address, string postalCode)
    {
        Name = name;
        LastName = lastName;
        Address = address;
        PostalCode = postalCode;
    }
}