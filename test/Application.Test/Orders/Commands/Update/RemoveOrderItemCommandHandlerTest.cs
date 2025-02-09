using Application.Application.Models;
using Application.Application.Orders.Commands.Update;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Application.Application.Products.Queries.GetById;
using Application.Test.Orders.Helpers;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using MediatR;
using Moq;

namespace Application.Test.Orders.Commands.Update;

[TestFixture]
public class RemoveOrderItemCommandHandlerTest
{
    private Mock<IOrderRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IMediator> _mediator;
    private RemoveOrderItemCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IOrderRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _mediator = new Mock<IMediator>();
        _handler = new RemoveOrderItemCommandHandler(_repository.Object, _unitOfWork.Object, _mediator.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_Delete_WhenValidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);
        
        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var command = new RemoveOrderItemCommand(orderId, product.Id);

        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(product.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(order);

        _repository
            .Setup(repo => repo.Delete(It.IsAny<Order>()));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.EqualTo(true));

        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Once);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        _repository.Verify(repo => repo.Delete(It.IsAny<Order>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldPass_Update_WhenValidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var removeProduct = ObjectFactory.ProductFactory("Smart Phone 1", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);
        var removeItem = ObjectFactory.ItemFactory(removeProduct, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item, removeItem });
        order.SetId(orderId);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var command = new RemoveOrderItemCommand(orderId, removeProduct.Id);

        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(removeProduct.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(order);

        _repository
            .Setup(repo => repo.Update(It.IsAny<Order>()));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.EqualTo(true));

        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Once);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        _repository.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenTotalPriceNotCalculated()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);
        
        var command = new RemoveOrderItemCommand(orderId, product.Id);

        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(product.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(order);

        _repository
            .Setup(repo => repo.Delete(It.IsAny<Order>()));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo("Decrease price must be lower than or equal to TotalPrice."));

        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Once);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        _repository.Verify(repo => repo.Delete(It.IsAny<Order>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}