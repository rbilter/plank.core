using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Plank.Core.Entities;
using X.PagedList;

namespace Plank.Core.Data
{
    public sealed class PlankRepository<TEntity>(PlankDbContext context) : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly PlankDbContext _context = context ?? throw new ArgumentNullException(nameof(context));

        public IRepository<TEntity> NextRepository { get; private set; } = new EndRepository<TEntity>();

        public async Task Add(TEntity entity)
        {
            await NextRepository.Add(entity).ConfigureAwait(false);

            _context.Set<TEntity>().Add(entity);
            await _context.SaveChanges().ConfigureAwait(false);
            _context.DetachAllEntities();
        }

        public async Task BulkAdd(IEnumerable<TEntity> entities)
        {
            await NextRepository.BulkAdd(entities).ConfigureAwait(false);

            _context.Set<TEntity>().AddRange(entities);
            await _context.SaveChanges().ConfigureAwait(false);
            _context.DetachAllEntities();
        }

        public async Task Delete(int id)
        {
            await NextRepository.Delete(id).ConfigureAwait(false);

            var item = await _context.Set<TEntity>().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (item == null) { return; }
            
            _context.Set<TEntity>().Attach(item);
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChanges().ConfigureAwait(false);
            _context.DetachAllEntities();
        }

        public async Task<TEntity> Get(int id)
        {
            var result = await _context.Set<TEntity>().AsNoTracking().SingleOrDefaultAsync(i => i.Id == id).ConfigureAwait(false);
            if (result != null)
            {
                return result;
            }

            return await NextRepository.Get(id).ConfigureAwait(false);
        }

        public IRepository<TEntity> RegisterNext(IRepository<TEntity> repository)
        {
            NextRepository = repository;
            return NextRepository;
        }

        public async Task<IPagedList<TEntity>> Search(Expression<Func<TEntity, bool>> expression, List<Expression<Func<TEntity, object>>>? includes = null, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Set<TEntity>().AsNoTracking().Where(expression);

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            var result = await query.OrderBy(e => e.Id).ToPagedListAsync(pageNumber, pageSize).ConfigureAwait(false);
            if (result != null)
            {
                return result;
            }

            return await NextRepository.Search(expression, includes, pageNumber, pageSize).ConfigureAwait(false);
        }

        public async Task Update(TEntity entity)
        {
            await NextRepository.Update(entity).ConfigureAwait(false);

            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChanges().ConfigureAwait(false);
            _context.DetachAllEntities();
        }
    }
}