using Application.API.Models.Orders;
using FluentValidation;

namespace Application.API.Validations.Orders;

/// <summary>
/// Represents a validator for <see cref="UpdateOrderItemRequest"/> ensuring required fields and length constraints.
/// </summary>
public class UpdateOrderItemRequestValidator : AbstractValidator<UpdateOrderItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveOrderItemRequestValidator"/> class.
    /// Defines validation rules for updating an item of an existing order request.
    /// </summary>
    public UpdateOrderItemRequestValidator()
    {
        RuleFor(s => s.ProductId)
           .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(s => s.QuantityOfProduct)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}