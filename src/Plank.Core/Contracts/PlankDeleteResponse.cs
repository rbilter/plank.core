namespace Plank.Core.Contracts
{
    public class PlankDeleteResponse(PlankValidationResultCollection validationResults)
    {
        public PlankDeleteResponse()
            : this([])
        {

        }

        public int Id { get; set; }

        public PlankValidationResultCollection ValidationResults { get; } = [.. validationResults];
    }
}