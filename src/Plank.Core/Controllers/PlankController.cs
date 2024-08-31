using System.Linq.Expressions;
using Plank.Core.Contracts;
using Plank.Core.Data;
using Plank.Core.Models;
using Plank.Core.Search;
using Plank.Core.Services;

namespace Plank.Core.Controllers
{
    public sealed class PlankController<TEntity> where TEntity : class, IEntity, new()
    {
        private readonly PlankService<TEntity> _service;

        public PlankController(PlankDbContext context)
        {
            var repo = new PlankRepository<TEntity>(context);

            var logger = new PlankLogger<TEntity>();
            _service = new PlankService<TEntity>(repo, logger);
        }

        public async Task<PlankPostResponse<TEntity>> Add(TEntity entity)
        {
            return await _service.Add(entity).ConfigureAwait(false);
        }

        public async Task<PlankBulkPostResponse<TEntity>> BulkAdd(IEnumerable<TEntity> entities)
        {
            return await _service.BulkAdd(entities).ConfigureAwait(false);
        }

        public async Task<PlankDeleteResponse> Delete(int id)
        {
            return await _service.Delete(id).ConfigureAwait(false);
        }

        public async Task<PlankGetResponse<TEntity>> Get(int id)
        {
            return await _service.Get(id).ConfigureAwait(false);
        }

        public async Task<PlankEnumerableResponse<TEntity>> Search(ISearchBuilder<TEntity> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            var expression = builder.Build();
            var includes = builder.Includes ?? [];
            var pageNumber = builder.PageNumber;
            var pageSize = builder.PageSize;

            return await _service.Search(expression, includes, pageNumber, pageSize).ConfigureAwait(false);
        }

        public async Task<PlankPostResponse<TEntity>> Update(TEntity entity)
        {
            return await _service.Update(entity).ConfigureAwait(false);
        }

        public async Task<PlankPostResponse<TEntity>> Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return await _service.Update(entity, properties).ConfigureAwait(false);
        }
    }
}