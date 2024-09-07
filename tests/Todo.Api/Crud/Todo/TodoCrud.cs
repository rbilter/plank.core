using Microsoft.EntityFrameworkCore;
using Plank.Core.Contracts;
using Plank.Core.Controllers;
using Plank.Core.Mappers;
using Todo.Api.Data;
using Todo.Api.Data.Models;
using Todo.Api.Dtos.Todo;

namespace Todo.Api.Crud.Todo
{
    public class TodoCrud
    {
        private readonly PlankController<TodoModel> _plankController;

        public TodoCrud(DbContextOptions<TodoContext> options, string csvFilePath)
        {
            _plankController = new PlankController<TodoModel>(new TodoContext(options, csvFilePath));
        }

        public async Task<ApiGetResponseDto<TodoDto>> Get(int id)
        {
            var response = await _plankController.Get(id);
            return PlankMapper<TodoModel, TodoDto>.Mapper.Map<ApiGetResponseDto<TodoDto>>(response);
        }

        public async Task<ApiPostResponseDto<TodoDto>> Add(TodoDto dto)
        {
            var item = PlankMapper<TodoModel, TodoDto>.Mapper.Map<TodoModel>(dto);
            var response = await _plankController.Add(item);
            return PlankMapper<TodoModel, TodoDto>.Mapper.Map<ApiPostResponseDto<TodoDto>>(response);
        }

        public async Task<ApiPostResponseDto<TodoDto>> Update(TodoDto dto)
        {
            var item = PlankMapper<TodoModel, TodoDto>.Mapper.Map<TodoModel>(dto);
            var response = await _plankController.Update(item);
            return PlankMapper<TodoModel, TodoDto>.Mapper.Map<ApiPostResponseDto<TodoDto>>(response);
        }

        public async Task<ApiDeleteResponseDto> Delete(int id)
        {
            var result = await _plankController.Delete(id);
            return PlankMapper<TodoModel, TodoDto>.Mapper.Map<ApiDeleteResponseDto>(result);
        }
    }
}