using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Plank.Core.Entities;

namespace Todo.Api.Data.Models
{
    [HasSelfValidation]
    public class TodoModel : PlankEntity
    {
        public bool IsCompleted { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}