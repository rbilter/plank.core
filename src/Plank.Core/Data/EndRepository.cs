using Plank.Core.Models;
using System.Linq.Expressions;
using X.PagedList;

namespace Plank.Core.Data
{
    public sealed class EndRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public IRepository<TEntity> NextRepository { get; } = null!;

        public Task AddAsync(TEntity _)
        {
            return Task.CompletedTask;
        }

        public Task BulkAddAsync(IEnumerable<TEntity> _)
        {
            return Task.CompletedTask;
        }

        public Task DeleteAsync(int _)
        {
            return Task.CompletedTask;
        }

        public Task<TEntity> GetAsync(int _)
        {
            return Task.FromResult<TEntity>(null!);
        }

        public IRepository<TEntity> RegisterNext(IRepository<TEntity> _)
        {
            return null!;
        }

        public Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> _, List<Expression<Func<TEntity, object>>>? _1, int _2, int _3)
        {
            return Task.FromResult<IPagedList<TEntity>>(null!);
        }

        public Task UpdateAsync(TEntity _)
        {
            return Task.CompletedTask;
        }
    }
}