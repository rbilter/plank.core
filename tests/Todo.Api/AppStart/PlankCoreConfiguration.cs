using Plank.Core.Mappers;
using Plank.Core.Validators;
using Todo.Api.Profiles;

namespace Todo.Api.AppStart
{
    public static class PlankCoreConfiguration
    {
        public static void Configure()
        {
            PlankValidatorConfiguration.RegisterAssemblyByPartialName("Todo.Api");
            PlankMapperConfiguration.RegisterProfile<MappingProfile>();
        }
    }
}