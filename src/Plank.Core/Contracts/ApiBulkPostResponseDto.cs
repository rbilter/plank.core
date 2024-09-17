namespace Plank.Core.Contracts
{
    public class ApiBulkPostResponseDto<TDto> where TDto : IDto
    {
        public IEnumerable<(TDto Item, ApiValidationResultsDto ValidationResults)> Items { get; set; } = [];
    }
}