using Application.Application.Customers.Queries.GetById;
using Application.Application.Models;
using Application.Application.Orders.Dtos;
using Application.Application.Orders.Mappers;
using Application.Application.Products.Queries.GetById;
using Core.Domain.Base;
using Core.Domain.Orders;
using MediatR;

namespace Application.Application.Orders.Commands.Create;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ObjectBaseResponse<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IMediator mediator)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<ObjectBaseResponse<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery(request.CustomerId), cancellationToken);

        var items = request.Items.Select(item => new Item(item.ProductId, item.QuantityOfProduct)).ToList();

        var order = new Order(customer.Data.Id, items);

        foreach (var item in request.Items)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(item.ProductId), cancellationToken);
            var priceOfItem = product.Data.Price * item.QuantityOfProduct;

            order.IncreaseTotalPrice(priceOfItem);
        }

        await _orderRepository.CreateAsync(order, cancellationToken);
        await _unitOfWork.SaveChangesAsync();

        var result = await _orderRepository.FindByIdAsync(order.Id);

        return new ObjectBaseResponse<OrderDto>(result.Map());
    }
}