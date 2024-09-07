using Microsoft.AspNetCore.Mvc;
using Plank.Core.Contracts;
using Todo.Api.Crud;
using Todo.Api.Dtos;
using Todo.Api.Search;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoCrud _todoCrud;
        private readonly TodoSearch _todoSearch;

        public TodoController(TodoCrud todoCrud, TodoSearch todoSearch)
        {
            _todoCrud = todoCrud;
            _todoSearch = todoSearch;
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiDeleteResponseDto), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _todoCrud.Delete(id));
        }        

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiGetResponseDto<TodoDto>), 200)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _todoCrud.Get(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiPostResponseDto<TodoDto>), 200)]
        public async Task<IActionResult> Post([FromBody] TodoDto item)
        {
            return Ok(await _todoCrud.Add(item));
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiPostResponseDto<TodoDto>), 200)]
        public async Task<IActionResult> Put([FromBody] TodoDto item)
        {
            return Ok(await _todoCrud.Update(item));
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiEnumerableResponseDto<TodoDto>), 200)]
        public async Task<IActionResult> Search([FromBody] TodoSearchRequestDto item)
        {
            return Ok(await _todoSearch.Search(item));
        }
    }
}