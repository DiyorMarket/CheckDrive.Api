using AutoMapper;
using CheckDrive.Api.Controllers.Reviews;
using CheckDrive.Application.DTOs.Review;
using CheckDrive.Domain.Common;
using CheckDrive.Infrastructure.Persistence;
using CheckDrive.Tests.Unit.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CheckDrive.Tests.Unit.Architecture;

public abstract class ArchitectureTestBase : UnitTestBase
{
    protected readonly Assembly DomainAssembly = typeof(EntityBase).Assembly;
    protected readonly Assembly ApplicationAssembly = typeof(ReviewDtoBase).Assembly;
    protected readonly Assembly InfrastructureAssembly = typeof(CheckDriveDbContext).Assembly;
    protected readonly Assembly ApiAssembly = typeof(DoctorReviewsController).Assembly;

    protected ArchitectureTestBase()
    {
    }

    protected List<Type> GetEntities(bool inluceBaseEntities = false)
    {
        return DomainAssembly
            .GetTypes()
            .Where(t => (inluceBaseEntities || !t.IsAbstract) && !t.IsInterface)
            .Where(t => t.IsAssignableTo(typeof(EntityBase)))
            .ToList();
    }

    protected List<Type> GetEntityConfigurations()
    {
        return InfrastructureAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.HasGenericInterface(typeof(IEntityTypeConfiguration<>)))
            .ToList();
    }

    protected List<Type> GetMappings()
    {
        return ApplicationAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.IsAssignableTo(typeof(Profile)))
            .ToList();
    }

    protected List<Type> GetValidators()
    {
        return ApplicationAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.IsSubclassOfRawGeneric(typeof(AbstractValidator<>)))
            .ToList();
    }

    protected List<Type> GetServices()
    {
        return ApplicationAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.Name.EndsWith("Service"))
            .ToList();
    }

    protected List<Type> GetControllers()
    {
        return ApiAssembly
            .GetTypes()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .Where(t => t.IsAssignableTo(typeof(ControllerBase)))
            .ToList();
    }
}