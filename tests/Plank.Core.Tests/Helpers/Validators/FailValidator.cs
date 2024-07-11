using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Tests.Helpers.Entities;
using Plank.Core.Validators;

namespace Plank.Core.Tests.Helpers.Validators
{
    public class FailValidator : PlankValidator<ChildTwoEntity>
    {
        public override ValidationResults Validate(ChildTwoEntity item)
        {
            var result = new ValidationResults();
            result.AddResult(new ValidationResult("There was a problem", item, null, null, null));

            return result;
        }
    }
}