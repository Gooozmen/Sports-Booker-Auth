using System.Net;
using Application.Builders;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Shared.Commands;
using Shared.Enums;
using Shared.Responses;

namespace Tests.Presentation.Controllers;


public class AccountControllerTests
{
    private readonly Mock<IRequestHandler<CreateUserCommand,IdentityResult>> _mockUserCommandHandler;
    private readonly Mock<IRequestHandler<PasswordSignInCommand,SignInResponseBase>> _mockSignInCommandHandler;
    private readonly Mock<IHttpResponseBuilder> _mockResponseBuilder;
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IApplicationUserBuilder> _mockUserBuilder;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        // Mock dependencies
        _mockUserCommandHandler = new Mock<IRequestHandler<CreateUserCommand,IdentityResult>>();
        _mockResponseBuilder = new Mock<IHttpResponseBuilder>();
        _mockSignInCommandHandler = new Mock<IRequestHandler<PasswordSignInCommand,SignInResponseBase>>();
        _mockSender = new Mock<ISender>();
        _mockUserBuilder = new Mock<IApplicationUserBuilder>();

        // Initialize controller with mocks
        _controller = new AccountController
        (
            _mockSender.Object,
            _mockResponseBuilder.Object
        );
    }

    [Fact]
    public async Task ProcessUserRegistrationAsync_ShouldReturnOk_WhenUserCreationSucceeds()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure12@@13!" };
        var dataModel = new ApplicationUser { Email = command.Email, UserName = command.Email, Active = true };
        var successResult = IdentityResult.Success;

        _mockUserCommandHandler
            .Setup(u => u.Handle(command,CancellationToken.None))
            .ReturnsAsync(successResult);

        _mockResponseBuilder
            .Setup(r => r.CreateResponse((int)HttpStatusCode.Created,successResult, null))
            .Returns(new ControllerResponse<IdentityResult>
            {
                Message = HttpStatusDescriptions.GetDescription((int)HttpStatusCode.Created),
                StatusCode = (int)HttpStatusCode.Created,
                Data = IdentityResult.Success,
                IsSuccess = true
            });

        _mockUserBuilder
            .Setup(u => u.Apply(command))
            .Returns(dataModel);

        // Act
        var result = await _controller.ProcessUserRegistrationAsync(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task ProcessUserRegistrationAsync_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "", Password = "123" }; // Invalid
        _controller.ModelState.AddModelError("Email", "Email is required.");

        var expectedErrors = new List<string> { "Email is required." };

        _mockResponseBuilder
            .Setup(r => r.CreateResponse((int)HttpStatusCode.BadRequest, expectedErrors,null))
            .Returns(new ControllerResponse<List<string>>
            {
                IsSuccess = false,
                Message = HttpStatusDescriptions.GetDescription((int)HttpStatusCode.BadRequest),
                Data = expectedErrors,
                StatusCode = (int)HttpStatusCode.BadRequest
            });
        
        // Act
        var result = await _controller.ProcessUserRegistrationAsync(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task ProcessUserRegistrationAsync_ShouldReturnBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!" };
        var failedResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed" });

        _mockUserCommandHandler
            .Setup(u => u.Handle(command,CancellationToken.None))
            .ReturnsAsync(failedResult);

        _mockResponseBuilder
            .Setup(r => r.CreateResponse((int)HttpStatusCode.BadRequest, failedResult.Errors,null))
            .Returns( new ControllerResponse<IEnumerable<IdentityError>>
            {
                Message = HttpStatusDescriptions.GetDescription((int)HttpStatusCode.BadRequest),
                StatusCode = (int)HttpStatusCode.BadRequest,
                Data = failedResult.Errors,
                IsSuccess = false
            });

        // Act
        var result = await _controller.ProcessUserRegistrationAsync(command);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}