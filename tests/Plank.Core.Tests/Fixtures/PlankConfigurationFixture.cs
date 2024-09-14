using System.Reflection;
using Plank.Core.Configuration;

namespace Plank.Core.Tests.Fixtures
{
    public sealed class PlankConfigurationFixture : IDisposable
    {
        public PlankConfigurationFixture()
        {
            // Register the assembly containing the validators
            PlankAssemblyRegistrar.RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        public void Dispose()
        {
            // Clear the registered assemblies
            PlankAssemblyRegistrar.ClearRegisteredAssemblies();
        }
    }
}