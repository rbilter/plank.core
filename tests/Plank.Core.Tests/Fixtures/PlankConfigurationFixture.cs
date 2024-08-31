using System.Reflection;
using Plank.Core.Validators;

namespace Plank.Core.Tests.Fixtures
{
    public sealed class PlankConfigurationFixture : IDisposable
    {
        public PlankConfigurationFixture()
        {
            // Register the assembly containing the validators
            PlankConfiguration.RegisterAssembly(Assembly.GetExecutingAssembly());
        }

        public void Dispose()
        {
            // Clear the registered assemblies
            PlankConfiguration.ClearRegisteredAssemblies();
        }
    }
}