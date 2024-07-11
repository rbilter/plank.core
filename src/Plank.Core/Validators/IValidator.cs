using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Core.Validators
{
    public interface IValidator
    {
        int Priority { get; set; }

        ValidationResults Validate(object entity);
    }
}