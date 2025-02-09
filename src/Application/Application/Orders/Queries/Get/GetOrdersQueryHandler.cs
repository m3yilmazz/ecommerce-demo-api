using Application.Application.Models;
using Application.Application.Orders.Dtos;
using Application.Application.Orders.Mappers;
using Core.Domain.Orders;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Application.Orders.Queries.Get;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, ArrayBaseResponse<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ArrayBaseResponse<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var queryable = _orderRepository.FindAllAsQueryable();

        if (request.CustomerId.HasValue)
            queryable = queryable.Where(w => w.CustomerId == request.CustomerId);
        
        if (request.OrderDateStartAt.HasValue)
            queryable = queryable.Where(w => w.OrderDate >= request.OrderDateStartAt);
        
        if (request.OrderDateEndAt.HasValue)
            queryable = queryable.Where(w => w.OrderDate <= request.OrderDateEndAt);

        queryable = request.SortByOrderDateDescending
            ? queryable.OrderByDescending(w => w.OrderDate)
            : queryable.OrderBy(w => w.OrderDate);

        var totalData = await queryable.CountAsync(cancellationToken: cancellationToken);

        var result = await queryable
           .Skip(request.PageIndex * request.PageLength)
           .Take(request.PageLength)
           .ToListAsync(cancellationToken: cancellationToken);

        return new ArrayBaseResponse<OrderDto>(
            result.Select(s => s.Map()).ToList(),
            totalData,
            request.PageLength,
            request.PageIndex);
    }
}