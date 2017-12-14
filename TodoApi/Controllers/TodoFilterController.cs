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
        public async Task<IActionResult> GetById(int id)
        {
            if ((await _todoRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            return Ok(await _todoRepository.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Todo item)
        {
            if ((await _todoRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }

            item.Id = id;
            await _todoRepository.UpdateAsync(item);
            return Ok();
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
        public async Task<IActionResult> Delete(int id)
        {
            if ((await _todoRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            await _todoRepository.DeleteAsync(id);
            return Ok();
        }
    }
}
