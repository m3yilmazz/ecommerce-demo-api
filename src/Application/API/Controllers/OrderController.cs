using Application.API.Models.Orders;
using Application.Application.Models;
using Application.Application.Orders.Commands.Create;
using Application.Application.Orders.Commands.Delete;
using Application.Application.Orders.Commands.Update;
using Application.Application.Orders.Dtos;
using Application.Application.Orders.Queries.Get;
using Core.Domain.Errors.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers;

/// <summary>
/// Handles for both order-related and item-related create, read, update and delete (CRUD) operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used for sending commands and queries.</param>
    public OrderController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves a list of all orders if no filter provided. Retrieves a list of orders of a customer if customer ID filter provided.
    /// Also, sort by order date and iteration through pagination filters can be achieved if required filters will be provided.
    /// </summary>
    /// <param name="request">The request object containing customer ID and sort by order date filtering and pagination parameters.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ArrayBaseResponse{OrderDto}"/> contains list of orders matching the filters.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns a list of orders matching the filters.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ArrayBaseResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] GetOrdersRequest request)
    {
        return Ok(await Mediator.Send(new GetOrdersQuery(
            request.CustomerId,
            request.SortByOrderDateDescending,
            request.OrderDateStartAt,
            request.OrderDateEndAt,
            request.PageIndex,
            request.PageLength)));
    }

    /// <summary>
    /// Creates a new order for a customer with given item details.
    /// </summary>
    /// <param name="request">The request object containing order details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>201 Created - Returns an <see cref="ObjectBaseResponse{OrderDto}"/> which contains the created order details.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="201">Returns information of created order.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ObjectBaseResponse<OrderDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateOrderRequest request)
    {
        var response = await Mediator.Send(new CreateOrderCommand(
            request.CustomerId,
            request.Items
                .Select(item => new OrderItemDto(
                    item.ProductId,
                    item.QuantityOfProduct))
                .ToList()));

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Deletes an existing order with given ID.
    /// </summary>
    /// <param name="id">The unique identifer of the order to delete.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>204 No Content - If the order with provided ID deleted.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the order with given ID does not exist in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="204">No Content if the order with provided ID deleted.</response>
    /// <response code="404">Not Found if there is no order with provided ID.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteOrderCommand(id));
        return NoContent();
    }

    /// <summary>
    /// Adds new item or adjusting quantity of an existing item of an existing order.
    /// </summary>
    /// <param name="id">The unique identifer of the order to add item to it.</param>
    /// <param name="request">The request object containing item details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ObjectBaseResponse{OrderDto}"/> which contains the updated order details.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the order or the product with given ID does not exist in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns details of updated order.</response>
    /// <response code="404">Not Found if there is no order or product with provided ID.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPost("{id:Guid}/item")]
    [ProducesResponseType(typeof(ObjectBaseResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddOrderItem([FromRoute] Guid id, [FromBody] AddOrderItemRequest request)
    {
        return Ok(await Mediator.Send(new AddOrderItemCommand(
            id,
            request.ProductId,
            request.QuantityOfProduct)));
    }

    /// <summary>
    /// Updates the quantity of an existing item of an existing order.
    /// </summary>
    /// <param name="id">The unique identifer of the order to update the item of.</param>
    /// <param name="request">The request object containing item details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ObjectBaseResponse{OrderDto}"/> which contains the updated order details.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the order or the product or the item with given ID does not exist in database.</description></item>
    ///   <item><description>422 Unprocessable Entity - Returns an <see cref="ErrorResponse"/> if there is a violation of busiess rule about total price of the order.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns information of updated order.</response>
    /// <response code="404">Not Found if there is no order or product or item with provided ID.</response>
    /// <response code="422">Unprocessable Entity if there is violation of a business rule.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPut("{id:Guid}/item")]
    [ProducesResponseType(typeof(ObjectBaseResponse<OrderDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateOrderItem([FromRoute] Guid id, [FromBody] UpdateOrderItemRequest request)
    {
        return Ok(await Mediator.Send(new UpdateOrderItemCommand(
            id,
            request.ProductId,
            request.QuantityOfProduct)));
    }

    /// <summary>
    /// Deletes an existing item from an existing order. If the item to be deleted is the only item remaining in the order, the order will also be deleted.
    /// </summary>
    /// <param name="id">The unique identifer of the order to update the item of.</param>
    /// <param name="request">The request object containing details of the item to be deleted.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>204 No Content - If the item with provided ID deleted.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the order or the product or the item with given ID does not exist in database.</description></item>
    ///   <item><description>422 Unprocessable Entity - Returns an <see cref="ErrorResponse"/> if the product with given product ID exists in database but not in item list.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="204">No Content if the item with provided prodcut ID deleted.</response>
    /// <response code="404">Not Found if there is no order or product or item with provided ID.</response>
    /// <response code="422">Unprocessable Entity if there is violation of a business rule.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpDelete("{id:Guid}/item")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveOrderItem([FromRoute] Guid id, [FromBody] RemoveOrderItemRequest request)
    {
        await Mediator.Send(new RemoveOrderItemCommand(
            id,
            request.ProductId));
        return NoContent();
    }
}