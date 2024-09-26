using CheckDrive.Tests.Unit.Constants;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetArchTest.Rules;

namespace CheckDrive.Tests.Unit.Architecture;

public class EntityConfigurationTests : ArchitectureTestBase
{
    [Fact]
    public void Infrastructure_ShouldContain_AtLeastOneEntityConfiguration()
    {
        // Arrange
        var configurations = GetEntityConfigurations();

        // Act & Assert
        configurations.Should().NotBeEmpty("there should be at least one Entity Configuration.");
    }

    [Fact]
    public void Configurations_ShouldReside_InCorrespondingNamespace()
    {
        // Arrange

        // Act
        var result = Types.InAssembly(InfrastructureAssembly)
            .That()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .And()
            .AreNotAbstract()
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespace(Namespaces.EntityConfigurations)
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ConfigurationsNamespace_ShouldContain_OnlyEntityConfigurations()
    {
        // Arrange

        // Act
        var result = Types.InNamespace(Namespaces.EntityConfigurations)
            .That()
            .AreNotStatic()
            .Should()
            .ImplementInterface(typeof(IEntityTypeConfiguration<>))
            .And()
            .BeSealed()
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue();
    }

    [Fact(Skip = "Not Implemented yet")]
    public void EachEntity_ShouldHave_Configuration()
    {
        // Arrange
        var entities = GetEntities();
        var configurations = GetEntityConfigurations();

        // Assert & Assert
        configurations.Count.Should().Be(
            entities.Count,
            $"there should be {entities.Count} Entity Configurations");
    }

    [Fact]
    public void ConfigurationName_ShouldStartWith_CorrespondingEntityName()
    {
        // Arrange
        var entities = GetEntities();
        var configurations = GetEntityConfigurations();

        // Act & Assert
        foreach (var configuration in configurations)
        {
            entities.Should().Contain(
                entity => configuration.Name.StartsWith(entity.Name),
                $"configuration name: {configuration.Name} should start with an Entity name");
        }
    }

    [Fact]
    public void ConfigurationName_ShouldEndWith_Configuration()
    {
        // Arrange
        var configurations = GetEntityConfigurations();

        // Act & Assert
        configurations.Should().AllSatisfy(
            configuration => configuration.Name.Should().EndWith("Configuration"),
            $"configuration name should end with 'Configuration'");
    }
}
