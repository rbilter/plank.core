using AutoMapper;
using Plank.Core.Models;

namespace Plank.Core.Mappers
{
    internal static class Mapping<TEntity> where TEntity : IEntity
    {
        public static IMapper Mapper => Lazy.Value;

        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            var config = new MapperConfiguration(cfg => {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => (p.GetMethod?.IsPublic ?? false) || (p.GetMethod?.IsAssembly ?? false);
                cfg.AddProfile<MappingProfile<TEntity>>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
    }
}