using AutoMapper;
using VendingMachine.Presentation.Mapper;

namespace VendingMachine.Tests.AutoMapper
{
    public class AutoMapperTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AppMappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
