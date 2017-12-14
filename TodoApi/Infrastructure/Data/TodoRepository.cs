using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Interfaces;
using TodoApi.Models;

namespace TodoApi.Infrastructure.Data
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _dbContext;

        public TodoRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Todo todo)
        {
            if (!_dbContext.Todos.Any())
            {
                todo.Id = 1;
            }
            else
            {
                int maxId = _dbContext.Todos.Max(a => a.Id);
                todo.Id = maxId + 1;
            }
            _dbContext.Todos.Add(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var todo = _dbContext.Todos.FirstOrDefault(a => a.Id == id);
            _dbContext.Todos.Remove(todo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Todo> GetByIdAsync(int id)
        {
            return await _dbContext.Todos.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Todo>> ListAsync()
        {
            return await _dbContext.Todos.AsNoTracking().ToListAsync();
        }

        public async Task UpdateAsync(Todo todo)
        {
            _dbContext.Entry(todo).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
