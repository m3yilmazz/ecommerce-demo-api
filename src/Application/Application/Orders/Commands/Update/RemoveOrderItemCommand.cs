using MediatR;

namespace Application.Application.Orders.Commands.Update;

public class RemoveOrderItemCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }

    public RemoveOrderItemCommand(Guid orderId, Guid productId)
    {
        OrderId = orderId;
        ProductId = productId;
    }
}