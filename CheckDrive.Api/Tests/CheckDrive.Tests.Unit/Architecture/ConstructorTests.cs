using CheckDrive.Tests.Unit.Extensions;
using FluentAssertions;

namespace CheckDrive.Tests.Unit.Architecture;

public sealed class ConstructorTests : ArchitectureTestBase
{
    [Fact]
    public void ConstructorsOfAllControllers_ShouldThrowArgumentNullException_WhenAnyParameterIsNull()
    {
        // Arrange
        var controllers = GetControllers();

        // Act & Assert
        controllers.Should().NotBeEmpty("there should be at least one controller.");
        controllers.Should().AllSatisfy(
            _fixture.VerifyConstructors,
            "controller constructor should throw ArgumentNullException when passed 'null' value to any of its parameter");
    }

    [Fact]
    public void ConstructorsOfAllServices_ShouldThrowArgumentNullException_WhenAnyParameterIsNull()
    {
        // Arrange
        var services = GetServices();

        // Act & Assert
        services.Should().NotBeEmpty("there should be at least one service.");
        services.Should().AllSatisfy(
            _fixture.VerifyConstructors,
            "service constructor should throw ArgumentNullException when passed 'null' value to any of its parameter");
    }
}
