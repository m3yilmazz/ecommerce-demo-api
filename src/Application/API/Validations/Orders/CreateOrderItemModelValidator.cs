using Application.API.Models.Orders;
using FluentValidation;

namespace Application.API.Validations.Orders;

/// <summary>
/// Represents a validator for <see cref="CreateOrderItemModel"/> ensuring required fields and length constraints.
/// </summary>
public class CreateOrderItemModelValidator : AbstractValidator<CreateOrderItemModel>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderItemModelValidator"/> class.
    /// Defines validation rules for the items located in the order creation request.
    /// </summary>
    public CreateOrderItemModelValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");

        RuleFor(x => x.QuantityOfProduct)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}