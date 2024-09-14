using Plank.Core.Configuration;

namespace Todo.Api.AppStart
{
    public static class PlankCoreConfiguration
    {
        public static void Configure()
        {
            PlankAssemblyRegistrar.RegisterAssemblyByPartialName("Todo.Api");
        }
    }
}