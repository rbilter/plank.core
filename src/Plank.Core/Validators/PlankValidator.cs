using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Entities;

namespace Plank.Core.Validators
{
    public abstract class PlankValidator<TEntity> : IEntityValidator<TEntity> where TEntity : class, IEntity
    {
        public int Priority { get; set; }

        public abstract ValidationResults Validate(TEntity item);

        public ValidationResults Validate(object entity)
        {
            return Validate(entity as TEntity);
        }
    }
}