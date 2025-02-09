using Application.Application.Products.Queries.GetById;
using Core.Domain.Products;
using Moq;

namespace Application.Test.Products.Queries.GetById;

[TestFixture]
public class GetProductByIdQueryHandlerTest
{
    private Mock<IProductRepository> _repository;
    private GetProductByIdQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IProductRepository>();
        _handler = new GetProductByIdQueryHandler(_repository.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var productId = Guid.NewGuid();
        var name = "Smart Phone";
        var price = 100.75;

        var product = new Product(name, price);
        product.SetId(productId);

        var command = new GetProductByIdQuery(productId);

        _repository
            .Setup(repo => repo.FindByIdAsync(productId, default))
            .ReturnsAsync(product);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Id, Is.EqualTo(product.Id));
            Assert.That(result.Data.Name, Is.EqualTo(product.Name));
            Assert.That(result.Data.Price, Is.EqualTo(product.Price));
        });

        _repository.Verify(repo => repo.FindByIdAsync(productId, default), Times.Once);
    }
}