﻿using System;
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
            if ((await _todoRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }
            return Ok(await _todoRepository.GetByIdAsync(id));
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
            if ((await _todoRepository.ListAsync()).All(a => a.Id != id))
            {
                return NotFound(id);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            item.Id = id;
            await _todoRepository.UpdateAsync(item);
            return Ok();
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
