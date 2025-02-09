using Application.Application.Customers.Dtos;
using Application.Application.Customers.Mappers;
using Application.Application.Models;
using Core.Domain.Customers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Application.Customers.Queries.Get;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, ArrayBaseResponse<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomersQueryHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<ArrayBaseResponse<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var queryable = _customerRepository.FindAllAsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            queryable = queryable.Where(w => w.Name.Contains(request.Name));

        if (!string.IsNullOrWhiteSpace(request.LastName))
            queryable = queryable.Where(w => w.LastName.Contains(request.LastName));

        if (!string.IsNullOrWhiteSpace(request.Address))
            queryable = queryable.Where(w => w.Address.Contains(request.Address));

        if (!string.IsNullOrWhiteSpace(request.PostalCode))
            queryable = queryable.Where(w => w.PostalCode.Contains(request.PostalCode));

        var totalData = await queryable.CountAsync(cancellationToken: cancellationToken);

        var result = await queryable
           .Skip(request.PageIndex * request.PageLength)
           .Take(request.PageLength)
           .ToListAsync(cancellationToken: cancellationToken);

        return new ArrayBaseResponse<CustomerDto>(
            result.Select(s => s.Map()).ToList(),
            totalData,
            request.PageLength,
            request.PageIndex);
    }
}