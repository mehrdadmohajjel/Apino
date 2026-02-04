using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data.Seeds
{
    public static class OrderStatusTypeSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderStatusType>().HasData(
            new OrderStatusType { Id = 1, ShowName = "ثبت شده", EnglishName = "Created" },
            new OrderStatusType { Id = 2, ShowName = "پرداخت شده", EnglishName = "Paid" },
            new OrderStatusType { Id = 3, ShowName = "تایید شعبه", EnglishName = "Accepted" },
            new OrderStatusType { Id = 4, ShowName = "در حال آماده‌سازی", EnglishName = "Preparing" },
            new OrderStatusType { Id = 5, ShowName = " آماده تحویل ", EnglishName = "Done" },
            new OrderStatusType { Id = 6, ShowName = "لغو شده", EnglishName = "Canceled" },
            new OrderStatusType { Id = 7, ShowName = "در سبد خرید", EnglishName = "Draft" },
            new OrderStatusType { Id = 8, ShowName = "تحویل داده شده", EnglishName = "Deliverd" }
            );
        }
    }
}
