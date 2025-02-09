using Application.API.Controllers;
using Application.API.Models.Products;
using Application.Application.Models;
using Application.Application.Products.Commands.Create;
using Application.Application.Products.Commands.Delete;
using Application.Application.Products.Commands.Update;
using Application.Application.Products.Dtos;
using Application.Application.Products.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test.Products;

[TestFixture]
public class ProductControllerTest
{
    private Mock<IMediator> _mediatorMock;
    private ProductController _controller;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new ProductController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetProduct_WhenProductExists_ReturnsOK()
    {
        var name = "Smart Phone";
        var pageIndex = 0;
        var pageLength = 10;
        var price = 100.75;

        var request = new GetProductsRequest
        {
            Name = name,
            PageIndex = pageIndex,
            PageLength = pageLength
        };

        var mediatorResponse = new ArrayBaseResponse<ProductDto>(
            [
                new ProductDto(Guid.NewGuid(), name, price)
            ],
            1, 10, 0);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetProductsQuery>(q =>
                    q.Name == request.Name &&
                    q.PageIndex == request.PageIndex &&
                    q.PageLength == request.PageLength),
                default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Get(request);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        Assert.That(okResult!.Value, Is.EqualTo(mediatorResponse));
    }

    [Test]
    public async Task CreateProduct_WhenValidRequest_ReturnsCreated()
    {
        var name = "Smart Phone";
        var price = 100.75;

        var request = new CreateProductRequest
        {
            Name = name,
            Price = price
        };

        var mediatorResponse = new ObjectBaseResponse<ProductDto>(
            new ProductDto(
                Guid.NewGuid(),
                name,
                price));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateProductCommand>(), default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Create(request);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
    }

    [Test]
    public async Task UpdateProduct_WhenValidRequest_ReturnsOK()
    {
        var productId = Guid.NewGuid();
        var name = "Smart Phone";
        var price = 100.75;

        var request = new UpdateProductRequest
        {
            Name = name,
            Price = price
        };

        var mediatorResponse = new ObjectBaseResponse<ProductDto>(
            new ProductDto(
                productId,
                name,
                price));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Update(productId, request);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        var data = okResult!.Value as ObjectBaseResponse<ProductDto>;
        Assert.That(data!.Data.Id, Is.EqualTo(mediatorResponse.Data.Id));
    }

    [Test]
    public async Task DeleteProduct_WhenValidRequest_ReturnsNoContent()
    {
        var customerId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), default))
            .ReturnsAsync(true);

        var result = await _controller.Delete(customerId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}