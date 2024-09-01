namespace Plank.Core.Contracts
{
    public class ApiPostResponseDto<T> where T : IDto
    {
        public T Item { get; set; } = default!;

        public ApiValidationResultsDto ValidationResults { get; set; } = [];
    }
}