using CourtBooker.Auth.Infrastructure.Environments;
using Microsoft.AspNetCore.Hosting;
using Moq;

namespace CourtBooker.Auth.Tests.Infra.Environments;

public class EnvironmentValidatorTests
{
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly EnvironmentValidator _validator;

    public EnvironmentValidatorTests()
    {
        _envMock = new Mock<IWebHostEnvironment>();
        _validator = new EnvironmentValidator(_envMock.Object);
    }

    [Fact]
    public void IsDevelopment_ShouldReturnTrue_WhenEnvironmentIsDevelopment()
    {
        // Arrange
        _envMock.Setup(e => e.EnvironmentName).Returns("Development");

        // Act
        var result = _validator.IsDevelopment();

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsStaging_ShouldReturnTrue_WhenEnvironmentIsStaging()
    {
        _envMock.Setup(e => e.EnvironmentName).Returns("Staging");

        var result = _validator.IsStaging();

        Assert.True(result);
    }

    [Fact]
    public void IsProduction_ShouldReturnTrue_WhenEnvironmentIsProduction()
    {
        _envMock.Setup(e => e.EnvironmentName).Returns("Production");

        var result = _validator.IsProduction();

        Assert.True(result);
    }

    [Fact]
    public void IsEnvironment_ShouldReturnTrue_ForMatchingEnvironmentName()
    {
        _envMock.Setup(e => e.EnvironmentName).Returns("QA");

        var result = _validator.IsEnvironment("QA");

        Assert.True(result);
    }

    [Fact]
    public void IsEnvironment_ShouldReturnFalse_ForNonMatchingEnvironmentName()
    {
        _envMock.Setup(e => e.EnvironmentName).Returns("Production");

        var result = _validator.IsEnvironment("Testing");

        Assert.False(result);
    }
}