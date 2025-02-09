using Application.Application.Customers.Commands.Delete;
using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using Moq;

namespace Application.Test.Customers.Commands.Delete;

[TestFixture]
public class DeleteCustomerHandlerTest
{
    private Mock<ICustomerRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private DeleteCustomerCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<ICustomerRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new DeleteCustomerCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var customerId = Guid.NewGuid();
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";

        var customer = new Customer(name, lastName, address, postalCode);
        customer.SetId(customerId);

        var command = new DeleteCustomerCommand(customerId);

        _repository
            .Setup(repo => repo.Delete(It.IsAny<Customer>()));

        _repository
            .Setup(repo => repo.FindByIdAsync(customerId, default))
            .ReturnsAsync(customer);

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.That(result, Is.EqualTo(true));

        _repository.Verify(repo => repo.FindByIdAsync(customerId, default), Times.Once);
        _repository.Verify(repo => repo.Delete(It.IsAny<Customer>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void Handle_ShouldThrowException_WhenInvalidItemsProvided()
    {
        var existingCustomerIds = new[] { Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid() };

        var command = new DeleteCustomerCommand(Guid.NewGuid());

        _repository
            .Setup(repo => repo.Delete(It.Is<Customer>(q => existingCustomerIds.All(a => a != command.Id))))
            .Throws(new NotFoundException($"There is no customer with given {command.Id} ID."));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo($"There is no customer with given {command.Id} ID."));

        _repository.Verify(repo => repo.Delete(It.IsAny<Customer>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}