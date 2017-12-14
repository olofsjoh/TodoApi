using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
        // GET: api/values
        [HttpGet]
        public async Task<List<Todo>> GetAll()
        {
            return await _todoRepository.ListAsync();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public async Task<IActionResult> GetById(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(new ApiOkResponse(todo));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Todo item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _todoRepository.AddAsync(item);

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Todo item)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _todoRepository.UpdateAsync(todo);
            return Ok(new ApiOkResponse(todo));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null)
            {
                return NotFound(new ApiResponse(404, $"Todo not found with id {id}"));
            }
            await _todoRepository.DeleteAsync(id);
            return Ok(new ApiOkResponse($"Todo deleted with id {id}"));
        }
    }
}
