using Application.Application.Customers.Dtos;
using Application.Application.Customers.Mappers;
using Application.Application.Models;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Customers.Queries.GetById;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, ObjectBaseResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByIdQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ObjectBaseResponse<CustomerDto>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.FindByIdAsync(request.Id) ??
             throw new NotFoundException($"There is no customer with given {request.Id} ID.");

        return new ObjectBaseResponse<CustomerDto>(customer.Map());
    }
}