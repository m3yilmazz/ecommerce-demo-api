using Application.Application.Customers.Commands.Delete;
using Application.Application.Customers.Commands.Update;
using Core.Domain.Base;
using Core.Domain.Customers;
using Core.Domain.Errors.Exceptions;
using Moq;

namespace Application.Test.Customers.Commands.Update;

[TestFixture]
public class UpdateCustomerHandlerTest
{
    private Mock<ICustomerRepository> _repository;
    private Mock<IUnitOfWork> _unitOfWork;
    private UpdateCustomerCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<ICustomerRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _handler = new UpdateCustomerCommandHandler(_repository.Object, _unitOfWork.Object);
    }

    [Test]
    public async Task Handle_ShouldPass_WhenValidItemsProvided()
    {
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";

        var customer = new Customer(name, lastName, address, postalCode);
        customer.SetId(Guid.NewGuid());

        var command = new UpdateCustomerCommand(customer.Id, "John", lastName, address, postalCode);

        _repository
            .Setup(repo => repo.FindByIdAsync(customer.Id, default))
            .ReturnsAsync(customer);

        _repository
            .Setup(repo => repo.Update(It.IsAny<Customer>()))
            .Callback<Customer>(c => { c.SetId(customer.Id); c.SetName(command.Name); });

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Id, Is.EqualTo(customer.Id));
            Assert.That(result.Data.Name, Is.EqualTo(command.Name));
            Assert.That(result.Data.LastName, Is.EqualTo(command.LastName));
            Assert.That(result.Data.Address, Is.EqualTo(command.Address));
            Assert.That(result.Data.PostalCode, Is.EqualTo(command.PostalCode));
        });

        _repository.Verify(repo => repo.FindByIdAsync(customer.Id, default), Times.Once);
        _repository.Verify(repo => repo.Update(It.IsAny<Customer>()), Times.Once);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Test]
    [TestCase("E", "Smith", "10th Ave.", "45678", "Customer name must be more than at least 1 character long.")]
    [TestCase("Emma", "S", "10th Ave.", "45678", "Customer last name must be more than at least 1 character long.")]
    [TestCase("Emma", "Smith", "1", "45678", "Customer address must be more than at least 1 character long.")]
    [TestCase("Emaa", "Smith", "10th Ave.", "4", "Customer postal code must be more than at least 1 character long.")]
    public void Handle_ShouldThowException_WhenInvalidNameProvided(
        string nameInCommand, 
        string lastNameInCommand, 
        string addressInCommand, 
        string postalCodeInCommand,
        string errorMessage)
    {
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";

        var customer = new Customer(name, lastName, address, postalCode);
        customer.SetId(Guid.NewGuid());

        var command = new UpdateCustomerCommand(
            customer.Id,
            nameInCommand,
            lastNameInCommand, 
            addressInCommand,
            postalCodeInCommand);

        _repository
            .Setup(repo => repo.FindByIdAsync(customer.Id, default))
            .ReturnsAsync(customer);

        _repository
            .Setup(repo => repo.Create(It.Is<Customer>(q =>
                q.Name.Length <= 1 ||
                q.LastName.Length <= 1 ||
                q.Address.Length <= 1 ||
                q.PostalCode.Length <= 1)))
            .Throws(new BusinessRuleException(errorMessage));

        _unitOfWork
            .Setup(uow => uow.SaveChangesAsync())
            .ReturnsAsync(1);

        var ex = Assert.ThrowsAsync<BusinessRuleException>(async () => await _handler.Handle(command, CancellationToken.None));

        Assert.That(ex.Message, Is.EqualTo(errorMessage));

        _repository.Verify(repo => repo.FindByIdAsync(customer.Id, default), Times.Once);
        _repository.Verify(repo => repo.Update(It.IsAny<Customer>()), Times.Never);
        _unitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Never);
    }
}