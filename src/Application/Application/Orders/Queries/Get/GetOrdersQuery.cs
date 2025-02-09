using Application.Application.Base.Queries;
using Application.Application.Models;
using Application.Application.Orders.Dtos;
using MediatR;

namespace Application.Application.Orders.Queries.Get;

public class GetOrdersQuery : BaseQuery, IRequest<ArrayBaseResponse<OrderDto>>
{
    public Guid? CustomerId { get; set; }
    public bool SortByOrderDateDescending { get; set; }
    public DateTime? OrderDateStartAt { get; set; }
    public DateTime? OrderDateEndAt { get; set; }

    public GetOrdersQuery(
        Guid? customerId,
        bool sortByOrderDateDescending,
        DateTime? orderDateStartAt,
        DateTime? orderDateEndAt,
        int pageIndex,
        int pageLength) : base(pageIndex, pageLength)
    {
        CustomerId = customerId;
        SortByOrderDateDescending = sortByOrderDateDescending;
        OrderDateStartAt = orderDateStartAt;
        OrderDateEndAt = orderDateEndAt;
    }
}