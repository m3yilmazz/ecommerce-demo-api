using Application.API.Controllers;
using Application.API.Models.Orders;
using Application.Application.Models;
using Application.Application.Orders.Commands.Create;
using Application.Application.Orders.Commands.Update;
using Application.Application.Orders.Dtos;
using Application.Application.Orders.Mappers;
using Application.Application.Orders.Queries.Get;
using Application.Application.Products.Commands.Delete;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Orders;
using Core.Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test.Orders;

[TestFixture]
public class OrderControllerTest
{
    private Mock<IMediator> _mediatorMock;
    private OrderController _controller;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new OrderController(_mediatorMock.Object);
    }


    private Product ProductFactory(string name, double price)
    {
        var product = new Product(name, price);
        product.SetId(Guid.NewGuid());

        return product;
    }

    private Item ItemFactory(Product product, int quantity)
    {
        var item = new Item(product.Id, quantity);
        item.SetId(product.Id);
        item.SetProduct(product);

        return item;
    }

    [Test]
    public async Task GetOrders_WhenCustomerExists_ReturnsOK()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var product = ProductFactory("Product 101", 101);
        var items = new List<Item>
        {
            ItemFactory(product, 10)
        };

        var order = new Order(customerId, items);
        order.SetId(orderId);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var request = new GetOrdersRequest()
        {
            CustomerId = customerId,
            SortByOrderDateDescending = false,
            PageIndex = 0,
            PageLength = 10
        };

        var mediatorResponse = new ArrayBaseResponse<OrderDto>([ order.Map()], -1, 10, 0);

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetOrdersQuery>(), default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Get(request);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        Assert.That(okResult!.Value, Is.EqualTo(mediatorResponse));
    }

    [Test]
    public async Task CreateOrder_WhenValidRequest_ReturnsCreated()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var quantityOfProduct = 10;
        var product = ProductFactory("Product 101", 101);
        var items = new List<Item>
        {
            ItemFactory(product, quantityOfProduct)
        };

        var order = new Order(customerId, items);
        order.SetId(orderId);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var request = new CreateOrderRequest
        {
            CustomerId = customerId,
            Items =
            [
                new()
                {
                    ProductId = product.Id,
                    QuantityOfProduct = quantityOfProduct
                }
            ]
        };

        var mediatorResponse = new ObjectBaseResponse<OrderDto>(order.Map());

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<CreateOrderCommand>(q => q.CustomerId == customerId && q.Items.Any()),
                default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Create(request);

        Assert.That(result, Is.InstanceOf<ObjectResult>());

        var okResult = result as ObjectResult;
        Assert.That(okResult!.Value, Is.EqualTo(mediatorResponse));
    }

    [Test]
    public void CreateOrder_WhenInvalidItems_ThrowsException()
    {
        var request = new CreateOrderRequest
        {
            CustomerId = Guid.NewGuid(),
            Items = []
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<CreateOrderCommand>(q => !q.Items.Any()), default))
            .ThrowsAsync(new BusinessRuleException("An order must contain at least one item."));

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _controller.Create(request));

        Assert.That(ex.Message, Is.EqualTo("An order must contain at least one item."));
    }

    [Test]
    public async Task DeleteOrder_WhenValidRequest_ReturnsNoContent()
    {
        var orderId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default))
            .ReturnsAsync(true);

        var result = await _controller.Delete(orderId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task AddOrderItem_WhenValidRequest_ReturnsOK()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var quantityOfProduct = 10;
        var product = ProductFactory("Product 101", 101);
        var items = new List<Item>
        {
            ItemFactory(product, quantityOfProduct)
        };

        var order = new Order(customerId, items);
        order.AddItem(product.Id, quantityOfProduct);
        order.SetId(orderId);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var request = new AddOrderItemRequest
        {
            ProductId = product.Id,
            QuantityOfProduct = quantityOfProduct
        };

        var mediatorResponse = new ObjectBaseResponse<OrderDto>(order.Map());

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<AddOrderItemCommand>(q => 
                    q.OrderId == orderId && 
                    q.ProductId == product.Id && 
                    q.QuantityOfProduct == quantityOfProduct), 
                default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.AddOrderItem(orderId, request);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        Assert.That(okResult!.Value, Is.EqualTo(mediatorResponse));
    }

    [Test]
    public async Task UpdateOrderItem_WhenValidRequest_ReturnsOK()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var quantityOfProduct = 10;
        var newQuantity = 100;
        var product = ProductFactory("Product 101", 101);
        var item = ItemFactory(product, quantityOfProduct);
        var items = new List<Item>
        {
            item
        };

        var order = new Order(customerId, items);
        order.SetId(orderId);
        order.UpdateItem(item.ProductId, newQuantity);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var request = new UpdateOrderItemRequest
        {
            ProductId = product.Id,
            QuantityOfProduct = newQuantity
        };

        var mediatorResponse = new ObjectBaseResponse<OrderDto>(order.Map());

        _mediatorMock
            .Setup(m => m.Send(
                It.Is<UpdateOrderItemCommand>(q =>
                    q.OrderId == orderId &&
                    q.ProductId == product.Id &&
                    q.QuantityOfProduct == newQuantity),
                default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.UpdateOrderItem(orderId, request);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        Assert.That(okResult!.Value, Is.EqualTo(mediatorResponse));
    }

    [Test]
    public void UpdateOrderItem_WhenValidRequest_ThrowsException()
    {
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var quantityOfProduct = 10;
        var newQuantity = 100;
        var product = ProductFactory("Product 101", 101);
        var item = ItemFactory(product, quantityOfProduct);
        var items = new List<Item>
        {
            item
        };

        var order = new Order(customerId, items);
        order.SetId(orderId);
        order.UpdateItem(item.ProductId, newQuantity);

        var totalPrice = order.Items.Select(item => item.QuantityOfProduct * item.Product.Price).Sum();
        order.SetTotalPrice(totalPrice);

        var request = new UpdateOrderItemRequest
        {
            ProductId = Guid.NewGuid(),
            QuantityOfProduct = newQuantity
        };

        _mediatorMock
            .Setup(m => m.Send(It.Is<UpdateOrderItemCommand>(q => order.Items.All(item => item.ProductId != q.ProductId)), default))
            .ThrowsAsync(new BusinessRuleException("Item with the given ProductId was not found."));

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _controller.UpdateOrderItem(orderId, request));

        Assert.That(ex.Message, Is.EqualTo("Item with the given ProductId was not found."));
    }

    [Test]
    public async Task RemoveOrderItem_WhenValidRequest_ReturnsNoContent()
    {
        var orderId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        
        var request = new RemoveOrderItemRequest
        {
            ProductId = productId
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<RemoveOrderItemCommand>(), default))
            .ReturnsAsync(true);

        var result = await _controller.RemoveOrderItem(orderId, request);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}