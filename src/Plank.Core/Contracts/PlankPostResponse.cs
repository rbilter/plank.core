using Plank.Core.Entities;

namespace Plank.Core.Contracts
{
    public class PlankPostResponse<TEntity> where TEntity : IEntity, new()
    {
        public PlankPostResponse()
            : this([])
        {
        }
    
        public PlankPostResponse(PlankValidationResultCollection validationResults)
        {
            ValidationResults = validationResults;
        }
    
        public TEntity Item { get; set; } = new TEntity();
    
        public PlankValidationResultCollection ValidationResults { get; }
    }
}