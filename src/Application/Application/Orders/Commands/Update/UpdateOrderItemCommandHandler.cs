using Application.Application.Models;
using Application.Application.Orders.Dtos;
using Application.Application.Orders.Mappers;
using Application.Application.Products.Queries.GetById;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using MediatR;

namespace Application.Application.Orders.Commands.Update;

public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, ObjectBaseResponse<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public UpdateOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<ObjectBaseResponse<OrderDto>> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId) ??
           throw new NotFoundException($"There is no order with given {request.OrderId} ID.");

        var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId), cancellationToken) ??
           throw new NotFoundException($"There is no product with given {request.ProductId} ID.");

        var existingItem = order.Items.FirstOrDefault(f => f.ProductId == request.ProductId) ??
           throw new NotFoundException($"There is no item with given {request.ProductId} product ID related order with given {request.OrderId} order ID.");

        var currentQuantityOfProduct = existingItem.QuantityOfProduct;

        if (request.QuantityOfProduct == currentQuantityOfProduct)
            return new ObjectBaseResponse<OrderDto>(order.Map());

        if (request.QuantityOfProduct > currentQuantityOfProduct)
        {
            var addedQuantity = request.QuantityOfProduct - currentQuantityOfProduct;

            order.UpdateItem(request.ProductId, request.QuantityOfProduct);

            var priceIncrease = product.Data.Price * addedQuantity;
            order.IncreaseTotalPrice(priceIncrease);
        }
        else if (request.QuantityOfProduct < currentQuantityOfProduct)
        {
            var removedQuantity = currentQuantityOfProduct - request.QuantityOfProduct;

            order.UpdateItem(request.ProductId, request.QuantityOfProduct);

            var priceDecrease = product.Data.Price * removedQuantity;
            order.DecreaseTotalPrice(priceDecrease);
        }

        order.SetUpdatedAt();

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        var result = await _orderRepository.FindByIdAsync(order.Id);

        return new ObjectBaseResponse<OrderDto>(result.Map());
    }
}