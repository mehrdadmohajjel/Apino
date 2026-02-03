using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Interfaces
{
    public interface IReadDbContext
    {
        DbSet<Branch> Branches { get; }
        DbSet<Domain.Entities.ProductCategory> ProductCategories { get; }
        DbSet<Product> Products { get; }
        DbSet<OtpCode> OtpCodes { get; }
        DbSet<Cart> Carts { get; }
        DbSet<CartItem> CartItems { get; }
        DbSet<Order> Orders { get; }
        DbSet<OrderDetail> OrderDetails { get; }
        DbSet<OrderStatus> OrderStatuses { get; }
        DbSet<OrderStatusType> OrderStatusTypes { get; }
        DbSet<User> Users { get; }
        DbSet<Notification> Notifications { get; }
        DbSet<BranchUser> BranchUsers { get; }
        DbSet<ServiceType> ServiceTypes { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
