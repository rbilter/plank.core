using FluentValidation;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Entities;

namespace Plank.Core.Validators
{
    internal sealed class FluentValidatorAdapter<TEntity>(AbstractValidator<TEntity> fluentValidator) : PlankValidator<TEntity> where TEntity : class, IEntity
    {
        private readonly AbstractValidator<TEntity> _fluentValidator = fluentValidator;

        public override ValidationResults Validate(TEntity item)
        {
            var validationResults = new ValidationResults();
            
            var results = _fluentValidator.Validate(item);
            foreach(var error in results.Errors)
            {
                validationResults.AddResult(new ValidationResult(error.ErrorMessage, null, error.PropertyName, error.ErrorCode, null));
            }

            return validationResults;
        }
    }
}