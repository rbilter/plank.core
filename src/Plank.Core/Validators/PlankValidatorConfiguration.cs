using System.Reflection;

namespace Plank.Core.Validators
{
    public static class PlankValidatorConfiguration
    {
        private static readonly List<Assembly> _registeredAssemblies = new List<Assembly>();

        public static void ClearRegisteredAssemblies()
        {
            _registeredAssemblies.Clear();
        }

        public static IEnumerable<Assembly> GetRegisteredAssemblies()
        {
            return _registeredAssemblies;
        }

        public static void RegisterAssembly(Assembly assembly)
        {
            if (!_registeredAssemblies.Contains(assembly))
            {
                _registeredAssemblies.Add(assembly);
            }
        }

        public static void RegisterAssemblyByPartialName(string partialName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName?.Contains(partialName) ?? false)
                .ToList();

            foreach (var assembly in assemblies)
            {
                RegisterAssembly(assembly);
            }
        }        
    }
}