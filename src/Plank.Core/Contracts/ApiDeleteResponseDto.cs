namespace Plank.Core.Contracts
{
    public class ApiDeleteResponseDto
    {
        public ApiValidationResultsDto ValidationResults { get; set; } = new();
    }
}