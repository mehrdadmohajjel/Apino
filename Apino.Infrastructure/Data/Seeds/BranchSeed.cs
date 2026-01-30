using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data.Seeds
{
    public static  class BranchSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // نمونه کارمندان شعبه
            modelBuilder.Entity<Branch>().HasData(
                new Branch
                {
                    Id = 1,
                    Title = "شعبه ولیعصر",
                    Address="تبریز - ولیعصر-",
                    Slug="شعبه-ولیعصر-تبریز",
                    Description="اولین شعبه آپینو",
                    ImageUrl="valiasr.jpg",
                    IsActive=true,
                   
                },
            new Branch
            {
                Id = 2,
                Title = "شعبه برج بلور",
                Address = "تبریز - آبرسان-فلکه دانشگاه-",
                Slug = "شعبه-بلور-تبریز",
                Description = "دومین شعبه آپینو",
                ImageUrl = "bloor-tower.jpg",
                IsActive = true,
            }
            );
        }

    }
}
