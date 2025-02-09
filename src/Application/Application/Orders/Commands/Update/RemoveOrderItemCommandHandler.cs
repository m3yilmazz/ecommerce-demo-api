using Application.Application.Products.Queries.GetById;
using Core.Domain.Base;
using Core.Domain.Orders;
using Core.Domain.Errors.Exceptions;
using MediatR;

namespace Application.Application.Orders.Commands.Update;

public class RemoveOrderItemCommandHandler : IRequestHandler<RemoveOrderItemCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<bool> Handle(RemoveOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId) ??
          throw new NotFoundException($"There is no order with given {request.OrderId} ID.");

        _ = await _mediator.Send(new GetProductByIdQuery(request.ProductId), cancellationToken) ??
           throw new NotFoundException($"There is no product with given {request.ProductId} ID.");

        order.RemoveItem(request.ProductId);

        if (order.Items.Count > 0)
            _orderRepository.Update(order);
        else
            _orderRepository.Delete(order);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}