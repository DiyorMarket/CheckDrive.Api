using FluentAssertions;
using FluentAssertions.Primitives;

namespace CheckDrive.Tests.Api.Extensions;

internal static class FluentAssertionExtensions
{
    public static AndConstraint<DateTimeAssertions> BeCloseToUtcNow(this DateTimeAssertions assertions)
    {
        return assertions.BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(5));
    }
}
