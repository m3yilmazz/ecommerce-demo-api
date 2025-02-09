using Application.Application.Customers.Mappers;
using Application.Application.Models;
using Application.Application.Orders.Commands.Update;
using Application.Application.Orders.Dtos;
using Application.Application.Products.Dtos;
using Application.Application.Products.Mappers;
using Application.Application.Products.Queries.GetById;
using Application.Test.Orders.Helpers;
using Core.Domain.Base;
using Core.Domain.Orders;
using MediatR;
using Moq;

namespace Application.Test.Orders.Commands.Update;

[TestFixture]
public class AddOrderItemCommandHandlerTest
{
    private Mock<IOrderRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private Mock<IMediator> _mediator;
    private AddOrderItemCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IOrderRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _mediator = new Mock<IMediator>();
        _handler = new AddOrderItemCommandHandler(_repository.Object, _unitOfWork.Object, _mediator.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var customer = ObjectFactory.CustomerFactory();
        var quantity = 10;
        var product = ObjectFactory.ProductFactory("Smart Phone", 100);
        var addProduct = ObjectFactory.ProductFactory("Computer", 2550);
        var item = ObjectFactory.ItemFactory(product, quantity);

        var orderId = Guid.NewGuid();
        var order = new Order(customer.Id, new List<Item> { item });
        order.SetId(orderId);

        var items = new List<OrderItemDto>() { new(product.Id, quantity) };

        var command = new AddOrderItemCommand(orderId, addProduct.Id, quantity);

        var getProductByIdQueryResponse = new ObjectBaseResponse<ProductDto>(addProduct.Map());

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
                c.AddItem(addProduct.Id, quantity); 
                var addedItem = c.Items.First(f => f.ProductId == addProduct.Id);
                addedItem.SetProduct(addProduct);
            });

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.Data.Items.Any(a => a.ProductId == addProduct.Id), Is.EqualTo(true));

        _mediator.Verify(m => m.Send(It.IsAny<GetProductByIdQuery>(), default), Times.Once);
        _repository.Verify(repo => repo.FindByIdAsync(It.IsAny<Guid>(), default), Times.Exactly(2));
        _repository.Verify(repo => repo.Update(It.IsAny<Order>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}