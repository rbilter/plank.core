using Plank.Core.Contracts;

namespace Todo.Api.Dtos
{
    public class TodoDto : AbstractDto
    {
        public bool IsCompleted { get; set; }

        public string Title { get; set; } = string.Empty;
    }
}