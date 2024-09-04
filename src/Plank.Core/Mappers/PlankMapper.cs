using AutoMapper;
using Plank.Core.Contracts;
using Plank.Core.Entities;

namespace Plank.Core.Mappers
{
    public sealed class PlankMapper<TEntity, TDto>
        where TEntity : IEntity, new()
        where TDto : IDto
    {
        public static IMapper Mapper => Lazy.Value;
        
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => (p.GetMethod?.IsPublic ?? false) || (p.GetMethod?.IsAssembly ?? false);
                cfg.AddProfile<PlankMapperProfile<TEntity, TDto>>();

                foreach (var profile in PlankMapperConfiguration.Profiles)
                {
                    cfg.AddProfile(profile);
                }                
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

    }
}