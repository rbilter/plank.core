using AutoMapper;
using log4net;
using System.Reflection;

namespace Plank.Core.Mappers
{
    public class ProfileRegistrar
    {
        private readonly List<Profile> _profiles;
        private readonly ILog _logger;

        public ProfileRegistrar(List<Profile> profiles, ILog logger)
        {
            _profiles = profiles;
            _logger = logger;
        }

        public void RegisterProfiles(IEnumerable<Assembly> assemblies)
        {
            var allTypes = GetAllTypes(assemblies);

            RegisterAutoMapperProfiles(allTypes);
        }

        private IEnumerable<Type> GetAllTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(t => t.IsClass && !t.IsAbstract);
        }

        private void RegisterAutoMapperProfiles(IEnumerable<Type> allTypes)
        {
            var profileTypes = allTypes.Where(t => typeof(Profile).IsAssignableFrom(t));
            foreach (var type in profileTypes)
            {
                try
                {
                    if (_profiles.Any(p => p.GetType() == type))
                    {
                        continue;
                    }
                    
                    var profile = (Profile)Activator.CreateInstance(type);
                    _profiles.Add(profile);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }
        }
    }
}