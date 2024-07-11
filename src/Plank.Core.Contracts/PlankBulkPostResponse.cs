namespace Plank.Core.Contracts
{
    public class PlankBulkPostResponse<T>(IEnumerable<(T, PlankValidationResultCollection)> validationResults)
    {
        public PlankBulkPostResponse()
            : this([])
        {

        }

        public IEnumerable<(T Item, PlankValidationResultCollection ValidationResults)> Items { get; } = validationResults;
    }
}