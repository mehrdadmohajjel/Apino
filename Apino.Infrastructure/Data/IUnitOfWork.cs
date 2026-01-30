using Apino.Domain.Entities;
using Apino.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Branch> Branches { get; }
        IRepository<ProductCategory> Categories { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }


        Task<int> SaveChangesAsync();
    }
}
