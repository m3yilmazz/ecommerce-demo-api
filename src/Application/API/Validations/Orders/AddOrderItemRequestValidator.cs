using Application.API.Models.Orders;
using FluentValidation;

namespace Application.API.Validations.Orders;

/// <summary>
/// Represents a validator for <see cref="AddOrderItemRequest"/> ensuring required fields and length constraints.
/// </summary>
public class AddOrderItemRequestValidator : AbstractValidator<AddOrderItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddOrderItemRequestValidator"/> class.
    /// Defines validation rules for the item adding into existing order request.
    /// </summary>
    public AddOrderItemRequestValidator()
    {
        RuleFor(s => s.ProductId)
           .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(s => s.QuantityOfProduct)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}