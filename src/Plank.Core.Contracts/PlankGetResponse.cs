namespace Plank.Core.Contracts
{
    public class PlankGetResponse<T>(T item) where T : new()
    {
        public PlankGetResponse()
            : this(new T())
        {
        }

        public T Item { get; } = item;

        public bool IsValid { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}