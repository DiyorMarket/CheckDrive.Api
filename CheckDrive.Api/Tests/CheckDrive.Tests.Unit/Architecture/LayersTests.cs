using FluentAssertions;
using NetArchTest.Rules;

namespace CheckDrive.Tests.Unit.Architecture;

public class LayerTests : ArchitectureTestBase
{
    [Fact]
    public void DomainLayer_ShouldNotHave_AnyDependencies()
    {
        // Arrange

        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAll("Infrastructure", "Application", "Api")
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue("Domain layer should not depend on any layer");
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_ApiAndInfrastructureLayers()
    {
        // Arrange

        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAll("Infrastructure", "Api")
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue("Application layer should not depend on Infrastructure or API layer");
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOn_ApiAndApplicationLayers()
    {
        // Arrange

        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .Should()
            .NotHaveDependencyOnAll("Application", "Api")
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue("Infrastructure should not depend on Application or API layer");
    }
}
