using System.Net;
using CourtBooker.Auth.Application.Builders;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CourtBooker.Auth.Presentation.Controllers;
using CourtBooker.Auth.Presentation.Services;
using CourtBooker.Auth.Shared.Commands;
using CourtBooker.Auth.Shared.Enums;
using CourtBooker.Auth.Shared.Responses;
using CourtBooker.Auth.Shared.Responses.Auth;

namespace CourtBooker.Auth.Tests.Presentation.Controllers;

public class AuthControllerTests
{
    private readonly AuthController _controller;
    private readonly Mock<IHttpResponseBuilder> _mockResponseBuilder;
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<IUserIdentifyService> _mockUserIdentifyService;

    public AuthControllerTests()
    {
        // Mock dependencies
        _mockResponseBuilder = new Mock<IHttpResponseBuilder>();
        _mockSender = new Mock<ISender>();
        _mockServiceProvider  = new Mock<IServiceProvider>();
        _mockUserIdentifyService = new Mock<IUserIdentifyService>();
        
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IHttpResponseBuilder)))
            .Returns(_mockResponseBuilder.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IUserIdentifyService)))
            .Returns(_mockUserIdentifyService.Object);

        // Initialize controller with mocks
        _controller = new AuthController
        (
            _mockSender.Object,
            _mockServiceProvider.Object
        );
    }

    [Fact]
    public async Task ProcessUserLoginAsyncShouldReturnBadRequestWhenUserDoesntExist()
    {
        //arrange 
        var cmd = new LoginCommand
        {
            Email = "test@test.com",
            Password = "pasor*(&(#JJd",
        };

        var handlerResult = new LoginResponse().Failed("Authentication Failed - Invalid username or password.");
        
        _mockSender
            .Setup(u => u.Send(cmd, CancellationToken.None))
            .ReturnsAsync(handlerResult);
        
        //act
        var result = await _controller.ProcessUserLoginAsync(cmd);
        
        //assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
    
     [Fact]
    public async Task ProcessUserRegistrationAsync_ShouldReturnBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "Secure123!" };
        var failedResult = IdentityResult.Failed(new IdentityError { Description = "User creation failed" });

        _mockSender
            .Setup(u => u.Send(command, CancellationToken.None))
            .ReturnsAsync(failedResult);

        _mockResponseBuilder
            .Setup(r => r.CreateResponse((int)HttpStatusCode.BadRequest, failedResult.Errors))
            .Returns(new Response<IEnumerable<IdentityError>>
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
    [Fact]
    public async Task ProcessUserRegistrationAsync_ShouldReturn_Created_WhenUserCreatedSuccessfully()
    {
        // Arrange
        var command = new CreateUserCommand{Email = "test@example.com",Password = "P@ssw0rd!"};
        var successResult = IdentityResult.Success; // Simulating a successful identity result

        _mockSender
            .Setup(sender => sender.Send(It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(successResult);

        // Act
        var result = await _controller.ProcessUserRegistrationAsync(command);

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
    }

    [Fact]
    public async Task ProcessUserRegistrationAsync_ShouldReturn_BadRequest_WhenUserCreationFails()
    {
        // Arrange
        var command = new CreateUserCommand { Email = "test@example.com", Password = "P@ssw0rd!" };
        var failureResult = IdentityResult.Failed();

        _mockSender
            .Setup(sender => sender.Send(It.IsAny<CreateUserCommand>(), CancellationToken.None))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _controller.ProcessUserRegistrationAsync(command);

        // Assert
        Assert.False(result is null);
        Assert.IsType<BadRequestObjectResult>(result);
    }
}
