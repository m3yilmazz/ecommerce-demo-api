﻿using Application.API.Models.Products;
using FluentValidation;

namespace Application.API.Validations.Products;

/// <summary>
/// Represents a validator for <see cref="CreateProductRequest"/> ensuring required fields and length constraints.
/// </summary>
public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductRequestValidator"/> class.
    /// Defines validation rules for the product creation request.
    /// </summary>
    public CreateProductRequestValidator()
    {
        RuleFor(s => s.Name)
            .NotNull().NotEmpty().WithMessage("Name is required.")
            .MinimumLength(5).WithMessage("Name must be at least 5 characters.")
            .MaximumLength(100).WithMessage("Name must be at most 100 characters.");

        RuleFor(s => s.Price)
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to zero.");
    }
}