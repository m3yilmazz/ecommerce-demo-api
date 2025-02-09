using Application.Application.Products.Commands.Create;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;
using Moq;

namespace Application.Test.Products.Commands.Create;

[TestFixture]
public class CreateProductHandlerTest
{
    private Mock<IProductRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private CreateProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new CreateProductCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var productId = Guid.NewGuid();
        var name = "Smart Phone";
        var price = 100.75;

        var product = new Product(name, price);
        product.SetId(productId);

        var command = new CreateProductCommand(name, price);

        _repository
            .Setup(repo => repo.Create(It.IsAny<Product>()))
            .Callback<Product>(c => c.SetId(productId));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result.Data.Id, Is.Not.EqualTo(Guid.Empty));

        _repository.Verify(repo => repo.Create(It.IsAny<Product>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenInvalidNameProvided()
    {
        var name = "Car";
        var price = 100.75;

        var command = new CreateProductCommand(name, price);

        _repository
            .Setup(repo => repo.Create(It.Is<Product>(q => q.Name.Length < 5)))
            .Throws(new BusinessRuleException("Product name must be at least 5 characters long."));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo("Product name must be at least 5 characters long."));

        _repository.Verify(repo => repo.Create(It.IsAny<Product>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenInvalidPriceProvided()
    {
        var name = "Smart Phone";
        var price = -100.75;

        var command = new CreateProductCommand(name, price);

        _repository
            .Setup(repo => repo.Create(It.Is<Product>(q => q.Name.Length < 5)))
            .Throws(new ArgumentOutOfRangeException($"price must be zero or greater. (Parameter 'price')\r\nActual value was {price}."));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo($"price must be zero or greater. (Parameter 'price')\r\nActual value was {price}."));

        _repository.Verify(repo => repo.Create(It.IsAny<Product>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}