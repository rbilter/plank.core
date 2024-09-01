namespace Plank.Core.Contracts
{
    public interface ISearchDto
    {
        int Length { get; set; }

        int PageNumber { get; }

        string SearchValue { get; set; }

        int Start { get; set; }
    }
}