namespace Plank.Core.Contracts
{
    public class PlankPostResponse<T>(PlankValidationResultCollection validationResults) where T : new()
    {
        public PlankPostResponse()
            : this([])
        {
        }

        public T Item { get; set; } = new T();

        public PlankValidationResultCollection ValidationResults { get; } = [.. validationResults];
    }
}