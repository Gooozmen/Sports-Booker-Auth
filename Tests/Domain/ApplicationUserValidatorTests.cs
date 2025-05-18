using Domain.Models;
using Domain.Validators;
using FluentValidation.TestHelper;

namespace Tests.Domain;

public class ApplicationUserValidatorTests
{
    private readonly ApplicationUserValidator _validator;

    public ApplicationUserValidatorTests()
    {
        _validator = new ApplicationUserValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var user = new ApplicationUser { Id = Guid.Empty }; // Empty ID
        var result = _validator.TestValidate(user);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Is_Empty()
    {
        var user = new ApplicationUser { UserName = "" };
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("UserName is required.");
    }

    [Fact]
    public void Should_Have_Error_When_UserName_Exceeds_MaxLength()
    {
        var user = new ApplicationUser { UserName = new string('A', 257) }; // 257 characters
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage("UserName cannot exceed 256 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var user = new ApplicationUser { Email = "" };
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid_Format()
    {
        var user = new ApplicationUser { Email = "invalid-email" };
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email format.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Email_Is_Valid()
    {
        var user = new ApplicationUser { Email = "test@example.com" };
        var result = _validator.TestValidate(user);
        result.ShouldNotHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_PasswordHash_Is_Empty()
    {
        var user = new ApplicationUser { PasswordHash = "" };
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.PasswordHash)
            .WithErrorMessage("PasswordHash is required.");
    }

    [Fact]
    public void Should_Have_Error_When_PhoneNumber_Is_Invalid()
    {
        var user = new ApplicationUser { PhoneNumber = "invalid_phone" };
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.PhoneNumber)
            .WithErrorMessage("Invalid phone number format.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_PhoneNumber_Is_Valid()
    {
        var user = new ApplicationUser { PhoneNumber = "+1234567890" };
        var result = _validator.TestValidate(user);
        result.ShouldNotHaveValidationErrorFor(x => x.PhoneNumber);
    }

    [Fact]
    public void Should_Have_Error_When_AccessFailedCount_Is_Negative()
    {
        var user = new ApplicationUser { AccessFailedCount = -1 };
        var result = _validator.TestValidate(user);
        result.ShouldHaveValidationErrorFor(x => x.AccessFailedCount)
            .WithErrorMessage("AccessFailedCount cannot be negative.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_AccessFailedCount_Is_Zero_Or_Positive()
    {
        var user = new ApplicationUser { AccessFailedCount = 0 };
        var result = _validator.TestValidate(user);
        result.ShouldNotHaveValidationErrorFor(x => x.AccessFailedCount);
    }
}