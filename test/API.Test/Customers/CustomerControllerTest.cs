using Application.API.Controllers;
using Application.API.Models.Customers;
using Application.Application.Customers.Commands.Create;
using Application.Application.Customers.Commands.Delete;
using Application.Application.Customers.Commands.Update;
using Application.Application.Customers.Dtos;
using Application.Application.Customers.Queries.Get;
using Application.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace API.Test.Customers;

[TestFixture]
public class CustomerControllerTest
{
    private Mock<IMediator> _mediatorMock;
    private CustomerController _controller;

    [SetUp]
    public void Setup()
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new CustomerController(_mediatorMock.Object);
    }

    [Test]
    public async Task GetCustomer_WhenCustomerExists_ReturnsOK()
    {
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";
        var pageIndex = 0;
        var pageLength = 10;

        var request = new GetCustomersRequest
        {
            Name = name,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode,
            PageIndex = pageIndex,
            PageLength = pageLength
        };
        
        var mediatorResponse = new ArrayBaseResponse<CustomerDto>(
            [
                new CustomerDto(Guid.NewGuid(), name, lastName, address, postalCode)
            ], 
            1, 10, 0);

        _mediatorMock
            .Setup(m => m.Send(It.Is<GetCustomersQuery>(q => 
                    q.Name == request.Name && 
                    q.LastName == request.LastName && 
                    q.Address == request.Address &&
                    q.PostalCode == request.PostalCode &&
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
    public async Task CreateCustomer_WhenValidRequest_ReturnsCreated()
    {
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";

        var request = new CreateCustomerRequest
        {
            Name = name,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode
        };

        var mediatorResponse = new ObjectBaseResponse<CustomerDto>(
            new CustomerDto(
                Guid.NewGuid(),
                name,
                lastName,
                address,
                postalCode));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<CreateCustomerCommand>(), default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Create(request);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
    }

    [Test]
    public async Task UpdateCustomer_WhenValidRequest_ReturnsOK()
    {
        var customerId = Guid.NewGuid();
        var name = "Emma";
        var lastName = "Smith";
        var address = "10th Ave.";
        var postalCode = "45678";

        var request = new UpdateCustomerRequest
        {
            Name = name,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode
        };

        var mediatorResponse = new ObjectBaseResponse<CustomerDto>(
            new CustomerDto(
                customerId,
                name,
                lastName,
                address,
                postalCode));

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<UpdateCustomerCommand>(), default))
            .ReturnsAsync(mediatorResponse);

        var result = await _controller.Update(customerId, request);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());

        var okResult = result as OkObjectResult;
        var data = okResult!.Value as ObjectBaseResponse<CustomerDto>;
        Assert.That(data!.Data.Id, Is.EqualTo(mediatorResponse.Data.Id));
    }

    [Test]
    public async Task DeleteCustomer_WhenValidRequest_ReturnsNoContent()
    {
        var customerId = Guid.NewGuid();

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<DeleteCustomerCommand>(), default))
            .ReturnsAsync(true);

        var result = await _controller.Delete(customerId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }
}