namespace Plank.Core.Contracts
{
    public class ApiPostResponseDto<TDto> where TDto : IDto
    {
        public TDto Item { get; set; } = default!;

        public ApiValidationResultsDto ValidationResults { get; set; } = [];
    }
}