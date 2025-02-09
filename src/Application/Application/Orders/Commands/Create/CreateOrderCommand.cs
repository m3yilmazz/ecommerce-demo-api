using Application.Application.Models;
using Application.Application.Orders.Dtos;
using MediatR;

namespace Application.Application.Orders.Commands.Create;

public class CreateOrderCommand : IRequest<ObjectBaseResponse<OrderDto>>
{
    public Guid CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }

    public CreateOrderCommand(Guid customerId, List<OrderItemDto> items)
    {
        CustomerId = customerId;
        Items = items;
    }
}