using Application.Application.Models;
using Application.Application.Orders.Dtos;
using MediatR;
namespace Application.Application.Orders.Commands.Update;

public class UpdateOrderItemCommand : IRequest<ObjectBaseResponse<OrderDto>>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int QuantityOfProduct { get; set; }

    public UpdateOrderItemCommand(Guid orderId, Guid productId, int quantityOfProduct)
    {
        OrderId = orderId;
        ProductId = productId;
        QuantityOfProduct = quantityOfProduct;
    }
}