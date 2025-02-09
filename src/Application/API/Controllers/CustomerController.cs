using Application.API.Models.Customers;
using Application.Application.Customers.Commands.Create;
using Application.Application.Customers.Commands.Delete;
using Application.Application.Customers.Commands.Update;
using Application.Application.Customers.Dtos;
using Application.Application.Customers.Queries.Get;
using Application.Application.Models;
using Core.Domain.Errors.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers;

/// <summary>
/// Handles customer-related create, read, update and delete (CRUD) operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CustomerController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used for sending commands and queries.</param>
    public CustomerController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves a list of customers matched with the given filters.
    /// </summary>
    /// <param name="request">The request object containing filtering and pagination parameters.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ArrayBaseResponse{CustomerDto}"/> contains list of customers matching the filters.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns a list of customers matching the filters.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ArrayBaseResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] GetCustomersRequest request)
    {
        return Ok(await Mediator.Send(new GetCustomersQuery(
            request.Name,
            request.LastName,
            request.Address,
            request.PostalCode,
            request.PageIndex,
            request.PageLength)));
    }

    /// <summary>
    /// Creates a new customer with given details.
    /// </summary>
    /// <param name="request">The request object containing customer details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>201 Created - Returns an <see cref="ObjectBaseResponse{CustomerDto}"/> which contains the created customer details.</description></item>
    ///   <item><description>409 Conflict - Returns an <see cref="ErrorResponse"/> if the customer with given parameters are already exist in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="201">Returns information of created customer.</response>
    /// <response code="409">Conflict if there is another customer with same values.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ObjectBaseResponse<CustomerDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateCustomerRequest request)
    {
        var response = await Mediator.Send(new CreateCustomerCommand(
            request.Name,
            request.LastName,
            request.Address,
            request.PostalCode));

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Updates an existing customer with given details.
    /// </summary>
    /// <param name="id">The unique identifer of the customer to update.</param>
    /// <param name="request">The request object containing updated customer details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ObjectBaseResponse{CustomerDto}"/> which contains the updated customer details.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the customer with given ID does not exist in database.</description></item>
    ///   <item><description>409 Conflict - Returns an <see cref="ErrorResponse"/> if there is another customer with given parameters already exists in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns details of updated customer.</response>
    /// <response code="404">Not Found if there is no customer with provided ID.</response>
    /// <response code="409">Conflict if there is another customer with same values.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ObjectBaseResponse<CustomerDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCustomerRequest request)
    {
        return Ok(await Mediator.Send(new UpdateCustomerCommand(
            id,
            request.Name,
            request.LastName,
            request.Address,
            request.PostalCode)));
    }

    /// <summary>
    /// Deletes an existing customer with given ID.
    /// </summary>
    /// <param name="id">The unique identifer of the customer to delete.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>204 No Content - If the customer with provided ID deleted.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the customer with given ID does not exist in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="204">No Content if the customer with provided ID deleted.</response>
    /// <response code="404">Not Found if there is no customer with provided ID.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteCustomerCommand(id));
        return NoContent();
    }
}