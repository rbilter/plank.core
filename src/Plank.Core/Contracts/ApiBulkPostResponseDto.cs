namespace Plank.Core.Contracts
{
    public class ApiBulkPostResponseDto<T> where T : IDto
    {
        public IEnumerable<(T Item, ApiValidationResultsDto ValidationResults)> Items { get; set; } = [];
    }
}