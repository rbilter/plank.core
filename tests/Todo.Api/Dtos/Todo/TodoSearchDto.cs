using Plank.Core.Contracts;

namespace Todo.Api.Dtos.Todo
{
    public class TodoSearchDto : AbstractSearchDto
    {
        public string? Title { get; set; }
    }
}