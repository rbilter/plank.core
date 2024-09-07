using Plank.Core.Contracts;

namespace Todo.Api.Dtos
{
	public class TodoSearchRequestDto : AbstractSearchDto
	{
		public string? Title { get; set; }
	}
}