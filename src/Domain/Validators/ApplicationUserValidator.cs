using FluentValidation;
using Domain.Models;

namespace Domain.Validators;

public class ApplicationUserValidator : AbstractValidator<ApplicationUser>
{
    public ApplicationUserValidator()
    {
        // Id is required
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");

        // UserName is required and must not exceed 256 characters
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MaximumLength(256).WithMessage("UserName cannot exceed 256 characters.");

        // NormalizedUserName must not exceed 256 characters
        RuleFor(x => x.NormalizedUserName)
            .MaximumLength(256).WithMessage("NormalizedUserName cannot exceed 256 characters.")
            .When(x => !string.IsNullOrEmpty(x.NormalizedUserName));

        // Email is required and must be a valid email format
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters.")
            .EmailAddress().WithMessage("Invalid email format.");

        // NormalizedEmail must not exceed 256 characters
        RuleFor(x => x.NormalizedEmail)
            .MaximumLength(256).WithMessage("NormalizedEmail cannot exceed 256 characters.")
            .When(x => !string.IsNullOrEmpty(x.NormalizedEmail));

        // PasswordHash is required
        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("PasswordHash is required.");

        // PhoneNumber validation (if provided)
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$") // E.164 format
            .WithMessage("Invalid phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        // AccessFailedCount should not be negative
        RuleFor(x => x.AccessFailedCount)
            .GreaterThanOrEqualTo(0).WithMessage("AccessFailedCount cannot be negative.");
    }
}


