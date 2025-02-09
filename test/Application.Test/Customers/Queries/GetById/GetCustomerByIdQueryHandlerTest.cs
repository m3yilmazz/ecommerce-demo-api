using Application.Application.Customers.Queries.GetById;
using Core.Domain.Customers;
using Moq;

namespace Application.Test.Customers.Queries.GetById;

[TestFixture]
public class GetCustomerByIdQueryHandlerTest
{
    private Mock<ICustomerRepository> _repository;
    private GetCustomerByIdQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _repository = new Mock<ICustomerRepository>();
        _handler = new GetCustomerByIdQueryHandler(_repository.Object);
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

        var command = new GetCustomerByIdQuery(customerId);

        _repository
            .Setup(repo => repo.FindByIdAsync(customerId, default))
            .ReturnsAsync(customer);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Data.Id, Is.EqualTo(customer.Id));
            Assert.That(result.Data.Name, Is.EqualTo(customer.Name));
            Assert.That(result.Data.LastName, Is.EqualTo(customer.LastName));
            Assert.That(result.Data.Address, Is.EqualTo(customer.Address));
            Assert.That(result.Data.PostalCode, Is.EqualTo(customer.PostalCode));
        });

        _repository.Verify(repo => repo.FindByIdAsync(customerId, default), Times.Once);
    }
}