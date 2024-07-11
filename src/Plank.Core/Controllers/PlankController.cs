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

        public async Task<PlankPostResponse<TEntity>> AddAsync(TEntity entity)
        {
            return await _service.AddAsync(entity).ConfigureAwait(false);
        }

        public async Task<PlankBulkPostResponse<TEntity>> BulkAddAsync(IEnumerable<TEntity> entities)
        {
            return await _service.BulkAddAsync(entities).ConfigureAwait(false);
        }

        public async Task<PlankDeleteResponse> DeleteAsync(int id)
        {
            return await _service.DeleteAsync(id).ConfigureAwait(false);
        }

        public async Task<PlankGetResponse<TEntity>> GetAsync(int id)
        {
            return await _service.GetAsync(id).ConfigureAwait(false);
        }

        public async Task<PlankEnumerableResponse<TEntity>> SearchAsync(ISearchBuilder<TEntity> builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            var expression = builder.Build();
            var includes = builder.Includes ?? [];
            var pageNumber = builder.PageNumber;
            var pageSize = builder.PageSize;

            return await _service.SearchAsync(expression, includes, pageNumber, pageSize).ConfigureAwait(false);
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity)
        {
            return await _service.UpdateAsync(entity).ConfigureAwait(false);
        }

        public async Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            return await _service.UpdateAsync(entity, properties).ConfigureAwait(false);
        }
    }
}