using Plank.Core.Entities;

namespace Plank.Core.Contracts
{
    public class PlankGetResponse<TEntity> where TEntity : IEntity, new()
    {
        public PlankGetResponse()
            : this(new TEntity())
        {
        }

        public PlankGetResponse(TEntity item)
        {
            Item = item;
        }

        public TEntity Item { get; }

        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}