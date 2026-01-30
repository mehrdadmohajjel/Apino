using Apino.Domain.Entities;
using Apino.Infrastructure.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;


            Users = new Repository<User>(_context);
            Branches = new Repository<Branch>(_context);
            Categories = new Repository<ProductCategory>(_context);
            Products = new Repository<Product>(_context);
            Orders = new Repository<Order>(_context);
        }


        public IRepository<User> Users { get; }
        public IRepository<Branch> Branches { get; }
        public IRepository<ProductCategory> Categories { get; }
        public IRepository<Product> Products { get; }
        public IRepository<Order> Orders { get; }


        public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();


        public void Dispose()
        => _context.Dispose();
    }
}
