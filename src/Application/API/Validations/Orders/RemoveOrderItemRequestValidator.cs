using Application.API.Models.Orders;
using FluentValidation;

namespace Application.API.Validations.Orders;

/// <summary>
/// Represents a validator for <see cref="RemoveOrderItemRequest"/> ensuring required fields and length constraints.
/// </summary>
public class RemoveOrderItemRequestValidator : AbstractValidator<RemoveOrderItemRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveOrderItemRequestValidator"/> class.
    /// Defines validation rules for removing an item from an existing order request.
    /// </summary>
    public RemoveOrderItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID is required.");
    }
}