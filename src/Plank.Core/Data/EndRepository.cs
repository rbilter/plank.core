using Plank.Core.Entities;
using System.Linq.Expressions;
using X.PagedList;

namespace Plank.Core.Data
{
    public sealed class EndRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public IRepository<TEntity> NextRepository { get; } = null!;

        public Task Add(TEntity _)
        {
            return Task.CompletedTask;
        }

        public Task BulkAdd(IEnumerable<TEntity> _)
        {
            return Task.CompletedTask;
        }

        public Task Delete(int _)
        {
            return Task.CompletedTask;
        }

        public Task<TEntity> Get(int _)
        {
            return Task.FromResult<TEntity>(null!);
        }

        public IRepository<TEntity> RegisterNext(IRepository<TEntity> _)
        {
            return null!;
        }

        public Task<IPagedList<TEntity>> Search(Expression<Func<TEntity, bool>> _, List<Expression<Func<TEntity, object>>>? _1, int _2, int _3)
        {
            return Task.FromResult<IPagedList<TEntity>>(null!);
        }

        public Task Update(TEntity _)
        {
            return Task.CompletedTask;
        }
    }
}