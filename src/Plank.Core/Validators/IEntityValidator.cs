using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Models;

namespace Plank.Core.Validators
{
    public interface IEntityValidator<TEntity> : IValidator where TEntity : IEntity
    {
        ValidationResults Validate(TEntity item);
    }
}