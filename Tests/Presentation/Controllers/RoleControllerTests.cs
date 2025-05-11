using System.Net;
using Application.Builders;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Services;
using Shared.Commands;
using Shared.Responses;

namespace Tests.Presentation.Controllers;

public class RoleControllerTests
{
    private readonly RoleController _controller;
    private readonly Mock<IHttpResponseBuilder> _mockResponseBuilder;
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IUserIdentifyService> _mockUserIdentifyService;

    public RoleControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _mockResponseBuilder = new Mock<IHttpResponseBuilder>();
        _mockUserIdentifyService = new Mock<IUserIdentifyService>();

        var serviceProvider = new Mock<IServiceProvider>();
        serviceProvider.Setup(s => s.GetService(typeof(IHttpResponseBuilder)))
            .Returns(_mockResponseBuilder.Object);
        serviceProvider.Setup(s => s.GetService(typeof(IUserIdentifyService)))
            .Returns(_mockUserIdentifyService.Object);

        _controller = new RoleController(_mockSender.Object, serviceProvider.Object);
    }

    [Fact]
    public async Task ProcessRoleRegistrationAsync_ShouldReturnCreated_WhenRoleIsCreated()
    {
        // Arrange
        var result = IdentityResult.Success;
        var command = new CreateRoleCommand { Name = "Admin" };
        var endpointResult = new Response<IdentityResult> { IsSuccess = true };


        _mockSender.Setup(s => s.Send(command, CancellationToken.None)).ReturnsAsync(result);

        _mockResponseBuilder
            .Setup(rb => rb.CreateResponse((int)HttpStatusCode.Created, result, string.Empty))
            .Returns(endpointResult);

        // Act
        var actionResult = await _controller.ProcessRoleRegistrationAsync(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode); // Optional
    }

    [Fact]
    public async Task ProcessRoleRegistrationAsync_ShouldReturnBadRequest_WhenRoleCreationFails()
    {
        // Arrange
        var result = IdentityResult.Failed();
        var command = new CreateRoleCommand { Name = "Admin" };

        _mockSender.Setup(s => s.Send(command, CancellationToken.None)).ReturnsAsync(result);

        _mockResponseBuilder
            .Setup(rb => rb.CreateResponse((int)HttpStatusCode.BadRequest, result, "user creation failed"));
        // Act
        var actionResult = await _controller.ProcessRoleRegistrationAsync(command);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult);
        Assert.Equal((int)HttpStatusCode.BadRequest, badRequest.StatusCode);
    }
}