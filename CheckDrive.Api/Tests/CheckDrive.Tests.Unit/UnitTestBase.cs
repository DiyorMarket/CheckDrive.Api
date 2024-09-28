using AutoFixture;
using AutoFixture.AutoMoq;

namespace CheckDrive.Tests.Unit;

public abstract class UnitTestBase
{
    protected readonly Fixture _fixture;

    protected UnitTestBase()
    {
        _fixture = CreateFixture();
    }

    private static Fixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        fixture.Behaviors.Add(new NullRecursionBehavior());
        fixture.Customize(new AutoMoqCustomization());

        return fixture;
    }
}
