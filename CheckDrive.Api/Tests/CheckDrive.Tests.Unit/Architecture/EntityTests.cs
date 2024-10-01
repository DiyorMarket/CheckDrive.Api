using CheckDrive.Domain.Common;
using CheckDrive.Tests.Unit.Constants;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace CheckDrive.Tests.Unit.Architecture;

public class EntityTests : ArchitectureTestBase
{
    [Fact]
    public void Domain_ShouldContain_AtLeastOneEntity()
    {
        // Arrange
        var entities = GetEntities();

        // Act & Assert
        entities.Should().NotBeEmpty("there should be at least one Entity");
    }

    [Fact]
    public void Entities_ShouldReside_InEntitiesNamespace()
    {
        // Arrange

        // Act
        var result = Types
            .InAssembly(DomainAssembly)
            .That()
            .Inherit(typeof(EntityBase))
            .And()
            .AreNotAbstract()
            .And()
            .AreNotInterfaces()
            .Should()
            .ResideInNamespace(Namespaces.Entities)
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue("entities should reside in 'Entities' namespace");
    }

    [Fact]
    public void EntitiesNamespace_ShouldContain_OnlyEntities()
    {
        // Arrange

        // Act
        var result = Types
            .InNamespace(Namespaces.Entities)
            .Should()
            .Inherit(typeof(EntityBase))
            .GetResult()
            .IsSuccessful;

        // Assert
        result.Should().BeTrue("all types in 'Entities' namespace should inherit EntityBase");
    }

    [Fact]
    public void AllNavigationProperties_ShouldBeVirtual()
    {
        // Arrange
        var properties = GetEntities()
            .SelectMany(t => t.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
            .Where(p => p.PropertyType.IsClass || p.PropertyType.IsInterface)
            .Where(p => p.PropertyType != typeof(string))
            .ToList();

        // Act & Assert
        foreach (var property in properties)
        {
            property.GetMethod!.IsVirtual.Should().BeTrue($"{property.DeclaringType}.{property.Name} must be declared as virtual");
        }
    }
}

