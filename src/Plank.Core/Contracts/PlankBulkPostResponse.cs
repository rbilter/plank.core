namespace Plank.Core.Contracts
{
    public class PlankBulkPostResponse<TEntity>(IEnumerable<(TEntity, PlankValidationResultCollection)> validationResults)
    {
        public PlankBulkPostResponse()
            : this([])
        {

        }

        public IEnumerable<(TEntity Item, PlankValidationResultCollection ValidationResults)> Items { get; } = validationResults;
    }
}