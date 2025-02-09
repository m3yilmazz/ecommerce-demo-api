using Application.API.Models.Orders;
using FluentValidation;

namespace Application.API.Validations.Orders;

/// <summary>
/// Represents a validator for <see cref="CreateOrderRequest"/> ensuring required fields and length constraints.
/// </summary>
public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateOrderRequestValidator"/> class.
    /// Defines validation rules for order creation request.
    /// </summary>
    public CreateOrderRequestValidator()
    {
        RuleFor(s => s.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required.");

        RuleFor(s => s.Items)
            .NotEmpty()
            .Must(m => m != null && m.Count > 0).WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items).SetValidator(new CreateOrderItemModelValidator());
    }
}