using Application.Application.Models;
using Application.Application.Orders.Dtos;
using Application.Application.Orders.Mappers;
using Application.Application.Products.Queries.GetById;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using MediatR;

namespace Application.Application.Orders.Commands.Update;

public class AddOrderItemCommandHandler : IRequestHandler<AddOrderItemCommand, ObjectBaseResponse<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public AddOrderItemCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<ObjectBaseResponse<OrderDto>> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.FindByIdAsync(request.OrderId) ??
           throw new NotFoundException($"There is no order with given {request.OrderId} ID.");

        var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId), cancellationToken) ??
           throw new NotFoundException($"There is no product with given {request.ProductId} ID.");

        order.AddItem(request.ProductId, request.QuantityOfProduct);

        var priceOfItem = product.Data.Price * request.QuantityOfProduct;

        order.IncreaseTotalPrice(priceOfItem);
        order.SetUpdatedAt();

        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        var result = await _orderRepository.FindByIdAsync(order.Id);

        return new ObjectBaseResponse<OrderDto>(result.Map());
    }
}