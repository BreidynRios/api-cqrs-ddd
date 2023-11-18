using Application.Commons.Mappings;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Context;

namespace Application.Test.Configurations.AutoMoq
{
    public class AutoFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Register(GetIConfiguration);
            fixture.Register(CreateContext);
            fixture.Register(CreateMapper);
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior(1));
            fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
            fixture.Customize(new AutoMoqCustomization());
        }

        private IConfiguration GetIConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

        private IManageEmployeesContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ManageEmployeesContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString(), opt => opt.EnableNullChecks(false))
                .Options;
            return new ManageEmployeesContext(options);
        }

        public IMapper CreateMapper()
        {
            var provider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return provider.CreateMapper();
        }
    }
}
