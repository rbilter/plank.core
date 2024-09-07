using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Plank.Core.Contracts;
using Plank.Core.Controllers;
using Plank.Core.Mappers;
using Plank.Core.Search;
using Todo.Api.Data;
using Todo.Api.Data.Models;
using Todo.Api.Dtos;

namespace Todo.Api.Search
{
    public class TodoSearch
    {
        private readonly PlankController<TodoModel> _plankController;

        public TodoSearch(DbContextOptions<TodoContext> options, string csvFilePath)
        {
            _plankController = new PlankController<TodoModel>(new TodoContext(options, csvFilePath));
        }

        public async Task<ApiEnumerableResponseDto<TodoDto>> Search(TodoSearchRequestDto item)
        {
            var builder = new SearchCriteriaBuilder<TodoModel>()
                .SetPageNumber(item.PageNumber)
                .SetPageSize(item.PageSize);

            if (item.Title != null)
            {
                builder.AddFilterAnd(p => p.Title.Contains(item.Title));
            }

            var response = await _plankController.Search(builder);
            return PlankMapper<TodoModel, TodoDto>.Mapper.Map<ApiEnumerableResponseDto<TodoDto>>(response);
        }
    }
}