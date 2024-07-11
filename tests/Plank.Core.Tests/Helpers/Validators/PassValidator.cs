using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Tests.Helpers.Entities;
using Plank.Core.Validators;

namespace Plank.Core.Tests.Helpers.Validators
{
    public class PassValidator : PlankValidator<ChildTwoEntity>
    {
        public override ValidationResults Validate(ChildTwoEntity item)
        {
            return new ValidationResults();
        }
    }
}