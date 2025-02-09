using Application.Application.Base.Commands;
using Application.Application.Customers.Dtos;
using Application.Application.Models;
using MediatR;

namespace Application.Application.Customers.Commands.Update;

public class UpdateCustomerCommand : BaseUpdateCommand, IRequest<ObjectBaseResponse<CustomerDto>>
{
    public string Name { get; }
    public string LastName { get; }
    public string Address { get; }
    public string PostalCode { get; }

    public UpdateCustomerCommand(Guid id, string name, string lastName, string address, string postalCode) : base(id)
    {
        Name = name;
        LastName = lastName;
        Address = address;
        PostalCode = postalCode;
    }
}