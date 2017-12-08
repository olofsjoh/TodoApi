using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.Todos.Count() == 0)
            {
                Setup();

                _context.SaveChanges();
            }
        }

        private void Setup()
        {
            _context.Todos.Add(new Todo { Id = 1, Name = "Start the dishwasher", IsComplete = true });
            _context.Todos.Add(new Todo { Id = 2, Name = "Buy milk", IsComplete = false });
            _context.Todos.Add(new Todo { Id = 3, Name = "Pay bills", IsComplete = true });
        }

        // GET: api/values
        [HttpGet]   
        public IEnumerable<Todo> GetAll()
        {
            return _context.Todos.ToList();
        }

        [HttpGet("{id}.{format?}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Create([FromBody] Todo item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Todos.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Todo item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.Todos.Update(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.Todos.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
