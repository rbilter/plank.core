using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Core.Validators;
using Todo.Api.Data.Models;

namespace Todo.Api.Validators
{
    public class TodoValidator : PlankValidator<TodoModel>
    {
        public override ValidationResults Validate(TodoModel model)
        {
            var result = new ValidationResults();

            if (string.IsNullOrWhiteSpace(model.Title))
            {
                result.AddResult(new ValidationResult("Title is required.", null, nameof(model.Title), "Required", null));
            }

            if (model.Title.Length > 25)
            {
                result.AddResult(new ValidationResult("Title must be less than 25 characters.", null, nameof(model.Title), "MaxLength", null));
            }

            return result;
        }
    }
}