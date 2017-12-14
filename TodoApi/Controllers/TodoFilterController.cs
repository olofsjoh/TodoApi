using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Filters;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ValidateModel]
    public class TodoFilterController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public TodoFilterController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }
        // GET: api/values
        [HttpGet]
        public async Task<List<Todo>> GetAll()
        {
            return await _todoRepository.ListAsync();
        }

        [HttpGet("{id}", Name = "GetFilterTodo")]
        [ValidateTodoExists]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _todoRepository.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        [ValidateTodoExists]
        public async Task<IActionResult> Update(int id, [FromBody] Todo item)
        {
            await _todoRepository.UpdateAsync(item);
            return Ok(new ApiOkResponse(item));
        }

        // POST api/values
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Todo item)
        {
            await _todoRepository.AddAsync(item);
            return CreatedAtRoute("GetFilterTodo", new { id = item.Id }, item);
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ValidateTodoExists]
        public async Task<IActionResult> Delete(int id)
        {
            await _todoRepository.DeleteAsync(id);
            return Ok(new ApiOkResponse($"Todo deleted with id {id}"));
        }
    }
}
