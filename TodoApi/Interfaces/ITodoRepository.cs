using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Interfaces
{
    public interface ITodoRepository
    {
        Task<Todo> GetByIdAsync(int id);
        Task<List<Todo>> ListAsync();
        Task UpdateAsync(Todo todo);
        Task AddAsync(Todo todo);
        Task DeleteAsync(int id);
    }
}
