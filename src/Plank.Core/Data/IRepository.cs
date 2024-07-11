namespace Plank.Core.Data
{
    public interface IRepository<T> : IReadRepository<T>, IWriteRepository<T>
    {
        IRepository<T> NextRepository { get; }

        IRepository<T> RegisterNext(IRepository<T> nextRepository);
    }
}