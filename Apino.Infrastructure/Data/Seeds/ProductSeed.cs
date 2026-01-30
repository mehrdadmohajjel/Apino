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
    public static class ProductSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // نمونه کارمندان شعبه
            modelBuilder.Entity<Product>().HasData(
 new Product
 {
     Id = 1,
     ProductCategoryId = 2,
     ServiceTypeId = 1,
     BranchId = 1,
     Title = "اسپرسو (۱۰۰ عربیکا+80/20)",
     Price = 1090000,
     ImageName = "Esperso.jpg",
     Description = "37 گرم عصاره قهوه",
     Stock = 20,
     IsActive = true,
 },
        new Product
        {
            Id = 2,
            ProductCategoryId = 2,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "آمریکانو (۱۰۰ عربیکا+80/20)",
            Price = 1090000,
            ImageName = "Americano.jpg",
            Description = "37 گرم عصاره قهوه + 220 گرم آب جوش",
            Stock = 20,
            IsActive = true,
        },

        new Product
        {
            Id = 3,
            ProductCategoryId = 2,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "اسپرسو کیندر",
            Price = 2090000,
            ImageName = "Kinder.jpg",
            Description = "37 گرم عصاره قهوه + شکلات کیندر",
            Stock = 30,
            IsActive = true,
        },
        new Product
        {
            Id = 4,
            ProductCategoryId = 2,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "قهوه دمی دستگاهی",
            Price = 1290000,
            ImageName = "lacP_V0rA2874922.jpg",
            Description = "220 گرم قهوه دم آوری شده دستگاهی",
            Stock = 20,
            IsActive = true,
        },
        new Product
        {
            Id = 5,
            ProductCategoryId = 7,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "آیس لاته وانیل",
            Price = 1590000,
            ImageName = "IceLateVanila.jpg",
            Description = "30 گرم سیروپ وانیل + 37 گرم عصاره قهوه + 220 گرم شیر فوم گرفته شده + یخ",
            Stock = 10,
            IsActive = true,
        },
        new Product
        {
            Id = 6,
            ProductCategoryId = 7,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "دمنوش سرماخوردگی",
            Price = 1480000,
            ImageName = "Damnoosh-floo.jpg",
            Description = "اویشن + لیمو + عسل + دارچین",
            Stock = 15,
            IsActive = true,
        },
        new Product
        {
            Id = 7,
            ProductCategoryId = 7,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "دمنوش زعفران",
            Price = 1580000,
            ImageName = "Safran-Drink.jpg",
            Description = "زعفران + هل سبز + گل محمدی + دارچین",
            Stock = 20,
            IsActive = true,
        },
        new Product
        {
            Id = 8,
            ProductCategoryId = 7,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "دگردیسی آپینو",
            Price = 1590000,
            ImageName = "DegarDisi.jpg",
            Description = "نعنا، سیروپ بلوکاراسئو، آب پرتقال، آب لیمو، آب انار",
            Stock = 30,
            IsActive = true,
        },
        new Product
        {
            Id = 9,
            ProductCategoryId = 7,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "نوشیدنی عرق بیدمشک",
            Price = 1080000,
            ImageName = "bidmeshkdrink.jpg",
            Description = "عرق بیدمشک + گلاب + زعفران",
            Stock = 10,
            IsActive = true,
        },
        new Product
        {
            Id = 10,
            ProductCategoryId = 6,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "نیمرو",
            Price = 1500000,
            ImageName = "nimroo.jpg",
            Description = "دو عدد تخم مرغ + نان",
            Stock = 10,
            IsActive = true,
        },
        new Product
        {
            Id = 11,
            ProductCategoryId = 6,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "املت گوجه",
            Price = 1900000,
            ImageName = "omlet.jpg",
            Description = "2 عدد تخم مرغ + 150 گرم سس گوجه + نان",
            Stock = 10,
            IsActive = true,
        },
        new Product
        {
            Id = 12,
            ProductCategoryId = 6,
            ServiceTypeId = 1,
            BranchId = 1,
            Title = "سیب زمینی رژیمی",
            Price = 2000000,
            ImageName = "diet-potato.jpg",
            Description = "سرخ شده / هواپز",
            Stock = 50,
            IsActive = true,
        }

            );
        }
    }
}
