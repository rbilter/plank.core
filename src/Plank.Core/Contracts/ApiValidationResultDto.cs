namespace Plank.Core.Contracts
{
    public class ApiValidationResultDto
    {
        public string Key { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public object Target { get; set; } = string.Empty;

        public IEnumerable<ApiValidationResultDto> NestedValidationResults { get; set; } = [];
    }
}