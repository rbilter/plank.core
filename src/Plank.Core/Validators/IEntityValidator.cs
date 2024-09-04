using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Entities;

namespace Plank.Core.Validators
{
    public interface IEntityValidator<TEntity> : IValidator where TEntity : IEntity
    {
        ValidationResults Validate(TEntity item);
    }
}