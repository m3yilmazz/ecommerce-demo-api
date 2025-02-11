using Application.Application.Products.Commands.Delete;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;
using Moq;

namespace Application.Test.Products.Commands.Delete;

[TestFixture]
internal class DeleteProductHandlerTest
{
    private Mock<IProductRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private DeleteProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteProductCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var productId = Guid.NewGuid();
        var name = "Smart Phone";
        var price = 100.75;

        var product = new Product(name, price);
        product.SetId(productId);

        var command = new DeleteProductCommand(productId);

        _repository
            .Setup(repo => repo.Delete(It.IsAny<Product>()));

        _repository
            .Setup(repo => repo.FindByIdAsync(productId, default))
            .ReturnsAsync(product);

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.EqualTo(true));

        _repository.Verify(repo => repo.FindByIdAsync(productId, default), Times.Once);
        _repository.Verify(repo => repo.Delete(It.IsAny<Product>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenInvalidItemsProvided()
    {
        var existingProductIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        var command = new DeleteProductCommand(Guid.NewGuid());

        _repository
            .Setup(repo => repo.Delete(It.Is<Product>(q => existingProductIds.All(a => a != command.Id))))
            .Throws(new NotFoundException($"There is no product with given {command.Id} ID."));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo($"There is no product with given {command.Id} ID."));

        _repository.Verify(repo => repo.Delete(It.Is<Product>(q => existingProductIds.All(a => a != command.Id))), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}