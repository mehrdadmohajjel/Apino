using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Apino.Infrastructure.Data.Seeds
{
    public static class UserSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Mobile = "09143010428",
                    IsActive = true,
                    Role = UserRole.SystemAdmin,
                    CreationDatetime = DateTime.UtcNow
                },
                new User
                {
                    Id = 2,
                    Mobile = "09359719229",
                    IsActive = true,
                    Role = UserRole.BranchAdmin,
                    CreationDatetime = DateTime.UtcNow
                },
                new User
                {
                    Id = 3,
                    Mobile = "09121234567",
                    IsActive = true,
                    Role = UserRole.Staff,
                    CreationDatetime = DateTime.UtcNow
                },
                    new User
                    {
                        Id = 4,
                        Mobile = "09127654321",
                        IsActive = true,
                        Role = UserRole.Staff,
                        CreationDatetime = DateTime.UtcNow
                    }
               );
        }
    }
}
