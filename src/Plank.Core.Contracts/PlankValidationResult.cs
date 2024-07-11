namespace Plank.Core.Contracts
{
    public class PlankValidationResult
    {
        public string Key { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string Tag { get; set; } = string.Empty;

        public object Target { get; set; } = new object();

        public IEnumerable<PlankValidationResult> NestedValidationResults { get; set; } = [];
    }
}