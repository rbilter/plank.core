namespace Plank.Core.Contracts
{
    public class ApiEnumerableResponseDto<TDto> where TDto : IDto
    {
        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public bool IsValid { get; set; }

        public IEnumerable<TDto> Items { get; set; } = [];

        public string Message { get; set; } = string.Empty;

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }
    }
}