using System.Net;
using Application.Builders;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;
using Presentation.Services;
using Shared.Commands;
using Shared.Enums;
using Shared.Responses;

namespace Tests.Presentation.Controllers;

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
        
        //act
        var result = await _controller.ProcessUserLoginAsync(cmd);
        
        //assert
        Assert.IsType<BadRequestResult>(result);
    }

}
