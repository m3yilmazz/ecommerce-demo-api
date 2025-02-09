using Application.API.Models.Products;
using Application.Application.Models;
using Application.Application.Products.Commands.Create;
using Application.Application.Products.Commands.Delete;
using Application.Application.Products.Commands.Update;
using Application.Application.Products.Dtos;
using Application.Application.Products.Queries.Get;
using Core.Domain.Errors.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.API.Controllers;

/// <summary>
/// Handles product-related create, read, update and delete (CRUD) operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used for sending commands and queries.</param>
    public ProductController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves a list of products matched with the given filters.
    /// </summary>
    /// <param name="request">The request object containing filtering and pagination parameters.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ArrayBaseResponse{ProductDto}"/> contains list of products matching the filters.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns a list of products matching the filters.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ArrayBaseResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] GetProductsRequest request)
    {
        return Ok(await Mediator.Send(new GetProductsQuery(
            request.Name,
            request.PageIndex,
            request.PageLength)));
    }

    /// <summary>
    /// Creates a new product with given details.
    /// </summary>
    /// <param name="request">The request object containing product details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>201 Created - Returns an <see cref="ObjectBaseResponse{ProductDto}"/> which contains the created product details.</description></item>
    ///   <item><description>409 Conflict - Returns an <see cref="ErrorResponse"/> if the customer with given parameters are already exist in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="201">Returns information of created product.</response>
    /// <response code="409">Conflict if there is another product with same values.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ObjectBaseResponse<ProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
    {
        var response = await Mediator.Send(new CreateProductCommand(
            request.Name,
            request.Price));

        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Updates an existing product with given details.
    /// </summary>
    /// <param name="id">The unique identifer of the product to update.</param>
    /// <param name="request">The request object containing updated product details.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>200 OK - Returns an <see cref="ObjectBaseResponse{CustomerDto}"/> which contains the updated product details.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the product with given ID does not exist in database.</description></item>
    ///   <item><description>409 Conflict - Returns an <see cref="ErrorResponse"/> if there is another product with given name already exists in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns details of updated product.</response>
    /// <response code="404">Not Found if there is no product with provided ID.</response>
    /// <response code="409">Conflict if there is another product with same name.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpPut("{id:Guid}")]
    [ProducesResponseType(typeof(ObjectBaseResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductRequest request)
    {
        return Ok(await Mediator.Send(new UpdateProductCommand(
            id,
            request.Name,
            request.Price)));
    }

    /// <summary>
    /// Deletes an existing product with given ID.
    /// </summary>
    /// <param name="id">The unique identifer of the product to delete.</param>
    /// <returns>
    /// A <see cref="Task{IActionResult}"/> containing one of the following:
    /// <list type="bullet">
    ///   <item><description>204 No Content - If the product with provided ID deleted.</description></item>
    ///   <item><description>404 Not Found - Returns an <see cref="ErrorResponse"/> if the product with given ID does not exist in database.</description></item>
    ///   <item><description>500 Internal Server Error - If an unexpected error occurs.</description></item>
    /// </list>
    /// </returns>
    /// <response code="204">No Content if the product with provided ID deleted.</response>
    /// <response code="404">Not Found if there is no product with provided ID.</response>
    /// <response code="500">Internal server error if something goes wrong.</response>
    [HttpDelete("{id:Guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await Mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
}