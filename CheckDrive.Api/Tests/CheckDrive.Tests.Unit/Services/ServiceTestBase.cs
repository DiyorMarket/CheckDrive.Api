using AutoMapper;
using Moq;

namespace CheckDrive.Tests.Unit.Services;

public abstract class ServiceTestBase : UnitTestBase
{
    protected readonly Mock<IMapper> _mapperMock;

    protected ServiceTestBase()
    {
        _mapperMock = new Mock<IMapper>();
    }

    public readonly static IEnumerable<object[]> InvalidIds = [[0], [-1], [-5], [-500], [int.MinValue]];
    public readonly static IEnumerable<object[]> ListCounts = [[1_000], [50], [25], [0]];
}