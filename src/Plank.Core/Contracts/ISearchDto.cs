namespace Plank.Core.Contracts
{
    public interface ISearchDto
    {
        int PageNumber { get; }

        int PageSize { get; set; }

        int Start { get; set; }
    }
}