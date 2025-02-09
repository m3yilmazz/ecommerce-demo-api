using Application.API.Models.Customers;
using FluentValidation;

namespace Application.API.Validations.Customers;

/// <summary>
/// Represents a validator for <see cref="CreateCustomerRequest"/> ensuring required fields and length constraints.
/// </summary>
public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateCustomerRequestValidator"/> class.
    /// Defines validation rules for the customer creation request.
    /// </summary>
    public CreateCustomerRequestValidator()
    {
        RuleFor(s => s.Name)
            .NotNull().NotEmpty().WithMessage("Name is required.")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

        RuleFor(s => s.LastName)
            .NotNull().NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters.")
            .MaximumLength(100).WithMessage("Last name must be at most 100 characters.");

        RuleFor(s => s.Address)
            .NotNull().NotEmpty().WithMessage("Address is required.")
            .MinimumLength(2).WithMessage("Address must be at least 2 characters.")
            .MaximumLength(100).WithMessage("Address must be at most 100 characters.");

        RuleFor(s => s.PostalCode)
            .NotNull().NotEmpty().WithMessage("Postal code is required.")
            .MinimumLength(2).WithMessage("Postal code must be at least 2 characters.")
            .MaximumLength(100).WithMessage("Postal code must be at most 100 characters.");
    }
}