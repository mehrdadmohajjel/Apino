using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Apino.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data
{
    public class AppDbContext : DbContext, IReadDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }


        public DbSet<Branch> Branches => Set<Branch>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ServiceType> ServiceTypes => Set<ServiceType>();
        public DbSet<User> Users => Set<User>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<OrderStatusType> OrderStatusTypes => Set<OrderStatusType>();
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<UserToken> UserTokens => Set<UserToken>();

        public DbSet<Cart> Carts => Set<Cart>();

        public DbSet<CartItem> CartItems => Set<CartItem>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();

        public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<BranchUser> BranchUsers => Set<BranchUser>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            ServiceTypeSeed.Seed(modelBuilder);
            CategorySeed.Seed(modelBuilder);
            OrderStatusTypeSeed.Seed(modelBuilder);
            BranchSeed.Seed(modelBuilder);
            UserSeed.Seed(modelBuilder);
            BranchStaffSeed.Seed(modelBuilder);
            ProductSeed.Seed(modelBuilder);


        }
    }
}
