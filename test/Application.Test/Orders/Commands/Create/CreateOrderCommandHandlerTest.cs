using Application.Application.Customers.Dtos;
using Application.Application.Customers.Mappers;
using Application.Application.Customers.Queries.GetById;
using Application.Application.Models;
using Application.Application.Orders.Commands.Create;
using Application.Application.Orders.Dtos;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Application.Application.Products.Queries.GetById;
using Application.Test.Orders.Helpers;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using MediatR;
using Moq;

namespace Application.Test.Orders.Commands.Create;

[TestFixture]
public class CreateOrderCommandHandlerTest
{
    private Mock<IOrderRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IMediator> _mediator;
    private CreateOrderCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IOrderRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _mediator = new Mock<IMediator>();
        _handler = new CreateOrderCommandHandler(_repository.Object, _unitOfWork.Object, _mediator.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);

        var items = new List<OrderItemDto>() { new(product.Id, quantity) };

        var command = new CreateOrderCommand(customer.Id, items);

        var getCustomerByIdQueryResponse = new ObjectBaseResponse<CustomerDto>(customer.Map());
        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(product.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default))
            .ReturnsAsync(getCustomerByIdQueryResponse);

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, cancellationToken) => order.SetId(Guid.NewGuid()));

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(order);

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.Data.Id, Is.Not.EqualTo(Guid.Empty));

        _mediator.Verify(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default), Times.Once);
        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Exactly(items.Count));
        _repository.Verify(repo => repo.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenInvalidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);

        var items = new List<OrderItemDto>();

        var command = new CreateOrderCommand(customer.Id, items);

        var getCustomerByIdQueryResponse = new ObjectBaseResponse<CustomerDto>(customer.Map());
        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(product.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default))
            .ReturnsAsync(getCustomerByIdQueryResponse);

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>((order, cancellationToken) => order.SetId(Guid.NewGuid()));

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(order);

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo("An order must contain at least one item."));

        _mediator.Verify(m => m.Send(It.IsAny<GetCustomerByIdQuery>(), default), Times.Once);
        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Never);
        _repository.Verify(repo => repo.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Never);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}