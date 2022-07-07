using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Database
{
    public interface IRepository<T> where T : class
    {
        Task UpdateAsync(int property, TodoModel model, CancellationToken token = default);

        Task DeleteAsync(long id, CancellationToken token = default);

        Task<long> AddAsync(TodoModel model, CancellationToken token = default);

        Task<T> GetAsync(long id, CancellationToken token = default);

        IAsyncEnumerable<T> GetAllAsync();
    }
}
