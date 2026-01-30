using Microsoft.EntityFrameworkCore;
using Apino.Domain.Entities;
namespace Apino.Infrastructure.Data.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query();
        Task<T?> GetByIdAsync(long id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
