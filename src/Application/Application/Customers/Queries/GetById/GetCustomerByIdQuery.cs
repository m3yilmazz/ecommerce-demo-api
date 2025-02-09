using Application.Application.Customers.Dtos;
using Application.Application.Models;
using MediatR;

namespace Application.Application.Customers.Queries.GetById;

public class GetCustomerByIdQuery : IRequest<ObjectBaseResponse<CustomerDto>>
{
    public Guid Id { get; set; }

    public GetCustomerByIdQuery(Guid id)
    {
        Id = id;
    }
}