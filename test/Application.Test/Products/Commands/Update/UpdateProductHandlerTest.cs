using Application.Application.Products.Commands.Update;
using Core.Domain.Base;
using Core.Domain.Errors.Exceptions;
using Core.Domain.Products;
using Moq;

namespace Application.Test.Products.Commands.Update;

[TestFixture]
public class UpdateProductHandlerTest
{
    private Mock<IProductRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private UpdateProductCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<IProductRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateProductCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var productId = Guid.NewGuid();
        var name = "Smart Phone";
        var price = 100.75;

        var product = new Product(name, price);
        product.SetId(productId);

        var command = new UpdateProductCommand(productId, "Smart Phone 1", 175.50);

        _repository
            .Setup(repo => repo.FindByIdAsync(productId, default))
            .ReturnsAsync(product);

        _repository
            .Setup(repo => repo.Update(It.IsAny<Product>()))
            .Callback<Product>(c =>
            {
                c.SetId(command.Id);
                c.SetName(command.Name);
                c.SetPrice(command.Price);
            });

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Id, Is.EqualTo(productId));
            Assert.That(result.Data.Name, Is.EqualTo(command.Name));
            Assert.That(result.Data.Price, Is.EqualTo(command.Price));
        });

        _repository.Verify(repo => repo.FindByIdAsync(productId, default), Times.Once);
        _repository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    [TestCase("Car", 150.75, "Product name must be at least 5 characters long.")]
    [TestCase("Smart Phone 1", -123.45, "Price must be greater than or equal to zero.")]
    public void Handle_ShouldThowException_WhenInvalidNameProvided(
        string nameInCommand,
        double priceInCommand,
        string errorMessage)
    {
        var productId = Guid.NewGuid();
        var name = "Smart Phone";
        var price = 100.75;

        var product = new Product(name, price);
        product.SetId(productId);

        var command = new UpdateProductCommand(
            productId,
            nameInCommand,
            priceInCommand);

        _repository
            .Setup(repo => repo.FindByIdAsync(productId, default))
            .ReturnsAsync(product);

        _repository
            .Setup(repo => repo.CreateAsync(It.Is<Product>(q =>
                q.Name.Length <= 1 ||
                q.Price < 0),
                It.IsAny<CancellationToken>()))
            .Throws(new BusinessRuleException(errorMessage));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));

        _repository.Verify(repo => repo.FindByIdAsync(productId, default), Times.Once);
        _repository.Verify(repo => repo.Update(It.IsAny<Product>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}