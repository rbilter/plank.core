using Plank.Core.Entities;

namespace Plank.Core.Services
{
    public interface IService<TEntity> : IReadService<TEntity>, IWriteService<TEntity> where TEntity : IEntity, new()
    {
    }
}