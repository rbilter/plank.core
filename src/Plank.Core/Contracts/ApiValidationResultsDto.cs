namespace Plank.Core.Contracts
{
    public class ApiValidationResultsDto : List<ApiValidationResultDto>
    {
        public bool IsValid { get { return this.Count == 0; } }
    }
}