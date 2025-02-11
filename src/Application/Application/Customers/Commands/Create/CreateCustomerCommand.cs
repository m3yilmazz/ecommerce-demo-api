using Application.Application.Base.Commands;
using Application.Application.Customers.Dtos;
using Application.Application.Models;
using Core.Domain.Customers;
using Core.Domain.Enums.Audit;
using MediatR;

namespace Application.Application.Customers.Commands.Create;

public class CreateCustomerCommand : IRequest<ObjectBaseResponse<CustomerDto>>, IAuditLogRequest
{
    public Guid Id { get; }
    public string Name { get; }
    public string LastName { get; }
    public string Address { get; }
    public string PostalCode { get; }

    public string EntityName => nameof(Customer);
    public Guid EntityId => Id;
    public string ActionType => ActionTypes.Create;
    public string OldValue => $"{nameof(Name)}: {null}, {nameof(LastName)}: {null}, {nameof(Address)}: {null}, {nameof(PostalCode)}: {null}";
    public string NewValue => $"{nameof(Name)}: {Name}, {nameof(LastName)}: {LastName}, {nameof(Address)}: {Address}, {nameof(PostalCode)}: {PostalCode}";

    public CreateCustomerCommand(string name, string lastName, string address, string postalCode)
    {
        Id = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Address = address;
        PostalCode = postalCode;
    }
}