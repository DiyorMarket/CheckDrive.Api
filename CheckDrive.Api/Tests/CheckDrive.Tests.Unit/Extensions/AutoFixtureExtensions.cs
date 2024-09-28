using AutoFixture;
using AutoFixture.Idioms;

namespace CheckDrive.Tests.Unit.Extensions;

internal static class AutoFixtureExtensions
{
    /// <summary>
    /// Creates a list of elements.
    /// </summary>
    public static List<T> CreateList<T>(this IFixture fixture, int itemsCount = 5)
    {
        var items = fixture.CreateMany<T>(itemsCount);

        return items.ToList();
    }

    /// <summary>
    /// Creates an array of elements.
    /// </summary>
    public static T[] CreateArray<T>(this IFixture fixture, int itemsCount = 5)
    {
        var items = fixture.CreateMany<T>(itemsCount);

        return items.ToArray();
    }

    /// <summary>
    /// Returns random exception object.
    /// </summary>
    public static Exception CreateException(this IFixture fixture)
    {
        var exception = fixture.Create<Exception>();

        return exception;
    }

    /// <summary>
    /// Fails if any of the constructor parameter is null, but exception is not thrown.
    /// </summary>
    public static void VerifyConstructors<T>(this IFixture fixture)
    {
        new GuardClauseAssertion(fixture).Verify(typeof(T).GetConstructors());
    }

    /// <summary>
    /// Fails if any of the constructor parameter is null, but exception is not thrown.
    /// </summary>
    public static void VerifyConstructors(this IFixture fixture, Type type)
    {
        new GuardClauseAssertion(fixture).Verify(type.GetConstructors());
    }
}
