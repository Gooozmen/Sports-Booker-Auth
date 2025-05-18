using Application.Behaviors;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Interfaces;

namespace Tests.Application.Behaviors;

public class LoggingPipelineBehaviorTests
{
    private readonly Mock<ILogger<LoggingPipelineBehavior<DummyRequest, DummyResponse>>> _loggerMock;

    public LoggingPipelineBehaviorTests()
    {
        _loggerMock = new Mock<ILogger<LoggingPipelineBehavior<DummyRequest, DummyResponse>>>();
    }

    [Fact]
    public async Task Handle_ShouldLogSuccess_WhenResponseIsSuccessful()
    {
        // Arrange
        var behavior = new LoggingPipelineBehavior<DummyRequest, DummyResponse>(_loggerMock.Object);
        var request = new DummyRequest();
        var response = new DummyResponse { IsSuccess = true };

        RequestHandlerDelegate<DummyResponse> next = () => Task.FromResult(response);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.Equal(response, result);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Handling request")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Completed Successfully")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldLogFailure_WhenResponseIsFailure()
    {
        // Arrange
        var behavior = new LoggingPipelineBehavior<DummyRequest, DummyResponse>(_loggerMock.Object);
        var request = new DummyRequest();
        var response = new DummyResponse { IsSuccess = false };

        RequestHandlerDelegate<DummyResponse> next = () => Task.FromResult(response);

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.Equal(response, result);

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failure")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}

public class DummyRequest : IRequest<DummyResponse> 
{
    public string Payload => "test-payload";
}

public class DummyResponse : IResponse
{
    public bool IsSuccess { get; set; }
}
