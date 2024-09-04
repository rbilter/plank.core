using Plank.Core.Entities;

namespace Plank.Core.Contracts
{
    public class PlankPostResponse<T> where T : IEntity, new()
    {
        public PlankPostResponse()
            : this([])
        {
        }
    
        public PlankPostResponse(PlankValidationResultCollection validationResults)
        {
            ValidationResults = validationResults;
        }
    
        public T Item { get; set; } = new T();
    
        public PlankValidationResultCollection ValidationResults { get; }
    }
}