using Plank.Core.Entities;

namespace Plank.Core.Contracts
{
    public class PlankGetResponse<T> where T : IEntity, new()
    {
        public PlankGetResponse()
            : this(new T())
        {
        }

        public PlankGetResponse(T item)
        {
            Item = item;
        }

        public T Item { get; }

        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}