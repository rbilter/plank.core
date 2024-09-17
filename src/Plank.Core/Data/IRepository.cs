namespace Plank.Core.Data
{
    public interface IRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity>
    {
        IRepository<TEntity> NextRepository { get; }

        IRepository<TEntity> RegisterNext(IRepository<TEntity> nextRepository);
    }
}