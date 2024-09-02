using Microsoft.AspNetCore.Mvc;
using Plank.Core.Contracts;
using Todo.Api.Crud;
using Todo.Api.Dtos;

namespace Todo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly TodoCrud _todoCrud;

        public TodoController(TodoCrud todoCrud)
        {
            _todoCrud = todoCrud;
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

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiDeleteResponseDto), 200)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _todoCrud.Delete(id));
        }
    }
}