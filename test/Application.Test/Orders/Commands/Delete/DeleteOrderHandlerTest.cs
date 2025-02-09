using Application.Application.Orders.Commands.Delete;
using Application.Test.Orders.Helpers;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using Moq;

namespace Application.Test.Orders.Commands.Delete;

[TestFixture]
public class DeleteOrderHandlerTest
{
    private Mock<IOrderRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private DeleteOrderCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IOrderRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteOrderCommandHandler(_repository.Object, _unitOfWork.Object);
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

        var command = new DeleteOrderCommand(orderId);

        _repository
            .Setup(repo => repo.Delete(It.IsAny<Order>()));

        _repository
            .Setup(repo => repo.FindByIdAsync(orderId, default))
            .ReturnsAsync(order);

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.EqualTo(true));

        _repository.Verify(repo => repo.FindByIdAsync(orderId, default), Times.Once);
        _repository.Verify(repo => repo.Delete(It.IsAny<Order>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenInvalidItemsProvided()
    {
        var existingOrderIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        var command = new DeleteOrderCommand(Guid.NewGuid());

        _repository
            .Setup(repo => repo.Delete(It.Is<Order>(q => existingOrderIds.All(a => a != command.Id))))
            .Throws(new NotFoundException($"There is no order with given {command.Id} ID."));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo($"There is no order with given {command.Id} ID."));

        _repository.Verify(repo => repo.Delete(It.IsAny<Order>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}