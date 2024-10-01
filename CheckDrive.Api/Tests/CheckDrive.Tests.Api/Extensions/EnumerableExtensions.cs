namespace CheckDrive.Tests.Api.Extensions;

internal static class EnumerableExtensions
{
    private static readonly Random _random = new();

    public static T GetRandomElement<T>(this IEnumerable<T> enumerable)
    {
        ArgumentNullException.ThrowIfNull(enumerable);

        var count = enumerable.Count();
        var randomIndex = _random.Next(count);

        return enumerable.ElementAt(randomIndex);
    }
}
