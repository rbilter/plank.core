using AutoMapper;
using log4net;
using Plank.Core.Configuration;
using Plank.Core.Contracts;
using Plank.Core.Entities;

namespace Plank.Core.Mappers
{
    public static class PlankMapper<TEntity, TDto>
            where TEntity : IEntity, new()
            where TDto : IDto
    {
        private static readonly ILog _logger = LogManager.GetLogger(nameof(PlankMapper<TEntity, TDto>));
        private static readonly List<Profile> _profiles = [];

        static PlankMapper()
        {
            var profileRegistrar = new ProfileRegistrar(_profiles, _logger);
            profileRegistrar.RegisterProfiles(PlankAssemblyRegistrar.GetRegisteredAssemblies());
        }

        public static IMapper Mapper => Lazy.Value;

        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => (p.GetMethod?.IsPublic ?? false) || (p.GetMethod?.IsAssembly ?? false);
                cfg.AddProfile<PlankMapperProfile<TEntity, TDto>>();

                foreach (var profile in _profiles)
                {
                    cfg.AddProfile(profile);
                }
            });

            return config.CreateMapper();
        });
    }
}