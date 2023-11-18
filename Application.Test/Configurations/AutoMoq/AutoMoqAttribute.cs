using AutoFixture;
using AutoFixture.Xunit2;

namespace Application.Test.Configurations.AutoMoq
{
    public class AutoMoqAttribute : AutoDataAttribute
    {
        public AutoMoqAttribute()
            : base(() => new Fixture().Customize(new AutoFixtureCustomization()))
        {
        }
    }
}
