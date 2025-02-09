using Application.Application.Models;
using Application.Application.Orders.Commands.Update;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Application.Application.Products.Queries.GetById;
using Application.Test.Orders.Helpers;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using Core.Domain.Products;
using MediatR;
using Moq;

namespace Application.Test.Orders.Commands.Update;

[TestFixture]
public class UpdateOrderItemCommandHandlerTest
{
    private Mock<IOrderRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IMediator> _mediator;
    private UpdateOrderItemCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IOrderRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _mediator = new Mock<IMediator>();
        _handler = new UpdateOrderItemCommandHandler(_repository.Object, _unitOfWork.Object, _mediator.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var newQuantity = 30;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var command = new UpdateOrderItemCommand(orderId, product.Id, newQuantity);

        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(product.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync(order);

        _repository
            .Setup(repo => repo.Update(It.IsAny<Order>()))
            .Callback<Order>(c =>
            {
                c.UpdateItem(product.Id, command.QuantityOfProduct);
                var updatedItem = c.Items.First(f => f.ProductId == product.Id);
                updatedItem.SetProduct(product);

                var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
                order.SetTotalPrice(totalPrice);
            });
        
        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Items.First(f => f.ProductId == product.Id).QuantityOfProduct, Is.EqualTo(newQuantity));
            Assert.That(result.Data.TotalPrice, Is.EqualTo(product.Price * newQuantity));
        });

        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Once);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Exactly(2));
        _repository.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenValidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var newQuantity = 30;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var command = new UpdateOrderItemCommand(orderId, product.Id, newQuantity);

        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(product.Map());

        _mediator
            .Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), default))
            .ReturnsAsync(getProductByIdQueryResponse);

        _repository
            .Setup(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default))
            .ReturnsAsync((Order)null);

        _repository
            .Setup(repo => repo.Update(It.IsAny<Order>()));
        
        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo($"There is no order with given {command.OrderId} ID."));

        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Never);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Once);
        _repository.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}