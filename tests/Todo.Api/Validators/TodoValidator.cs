using FluentValidation;
using Todo.Api.Data.Models;

namespace Todo.Api.Validators
{
    public class TodoValidator : AbstractValidator<TodoModel>
    {
        public TodoValidator()
        {
            RuleFor(todo => todo.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 25).WithMessage("Title must be between 1 and 25 characters.");
        }
    }
}