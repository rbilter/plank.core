using FluentValidation;
using Plank.Core.Tests.Helpers.Entities;

namespace Plank.Core.Tests.Helpers.Validators
{
    public class ChildThreeFluentValidator : AbstractValidator<ChildThreeEntity>
    {
        public ChildThreeFluentValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
}