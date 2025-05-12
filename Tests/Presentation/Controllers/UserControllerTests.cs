using CourtBooker.Auth.Application.Builders;
using MediatR;
using Moq;
using CourtBooker.Auth.Presentation.Controllers;
using CourtBooker.Auth.Presentation.Services;

namespace CourtBooker.Auth.Tests.Presentation.Controllers;

public class UserControllerTests
{
    private readonly UserController _controller;
    private readonly Mock<IHttpResponseBuilder> _mockResponseBuilder;
    private readonly Mock<ISender> _mockSender;
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<IUserIdentifyService> _mockUserIdentifyService;

    public UserControllerTests()
    {
        // Mock dependencies
        _mockResponseBuilder = new Mock<IHttpResponseBuilder>();
        _mockSender = new Mock<ISender>();
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockUserIdentifyService = new Mock<IUserIdentifyService>();

        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IHttpResponseBuilder)))
            .Returns(_mockResponseBuilder.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IUserIdentifyService)))
            .Returns(_mockUserIdentifyService.Object);

        // Initialize controller with mocks
        _controller = new UserController
        (
            _mockSender.Object,
            _mockServiceProvider.Object
        );
    }
}