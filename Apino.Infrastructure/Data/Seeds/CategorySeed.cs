using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apino.Infrastructure.Data.Seeds
{
    public static class CategorySeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // نمونه کارمندان شعبه
            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory
                {
                    Id = 1,
                    BranchId = 1,
                    CategoryTitle = "فضای کار اشتراکی",
                    Slug = "فضای-کار-اشتراکی",
                    IsActive = true,
                    PayAtPlace = true,
                    IconName = "place.png",
                    CreationDateTime = DateTime.Now,
                },
               
                new ProductCategory
                {
                    Id = 2,
                    BranchId = 1,
                    CategoryTitle = "کافه",
                    Slug = "کافه-1",
                    IsActive = true,
                    PayAtPlace = true,
                    IconName = "coffe.png",
                    CreationDateTime = DateTime.Now,

                },
                            new ProductCategory
                            {
                                Id = 3,
                                BranchId = 1,
                                CategoryTitle = "کافه خوردنی",
                                Slug = "کافه-خوردنی-1",
                                IsActive = true,
                                PayAtPlace = true,
                                IconName = "coffe.png",
                                CreationDateTime = DateTime.Now,

                            },
        new ProductCategory
        {
            Id = 4,
            BranchId = 1,
            CategoryTitle = "نوشیدنی سرد",
            Slug = "نوشیدنی-سرد-1",
            IsActive = true,
            PayAtPlace = true,
            IconName = "coffe.png",
            CreationDateTime = DateTime.Now,

        },
                new ProductCategory
                {
                    Id = 5,
                    BranchId = 1,
                    CategoryTitle = "فروشگاه",
                    Slug = "فروشگاه-1",
                    IsActive = true,
                    PayAtPlace = true,
                    IconName = "shop.png",
                    CreationDateTime = DateTime.Now,

                },
                new ProductCategory
                {
                    Id = 6,
                    BranchId = 1,
                    CategoryTitle = "میز مطالعه عمومی",
                    Slug = "میز-مطالعه-عمومی-1",
                    IsActive = true,
                    PayAtPlace = true,
                    IconName = "place.png",
                    CreationDateTime = DateTime.Now,

                },
            new ProductCategory
            {
                Id = 7,
                BranchId = 1,
                CategoryTitle = "استودیو",
                Slug = "استودیو-1",
                IsActive = true,
                PayAtPlace = true,
                IconName = "studio.png",
                CreationDateTime = DateTime.Now,

            },

                       new ProductCategory
                       {
                           Id = 8,
                           BranchId = 1,
                           CategoryTitle = "فضای کار اشتراکی",
                           Slug = "فضای-کار-اشتراکی",
                           IsActive = true,
                           PayAtPlace = true,
                           IconName = "place.png",
                           CreationDateTime = DateTime.Now,
                       },
                new ProductCategory
                {
                    Id = 9,
                    BranchId = 2,
                    CategoryTitle = "فضای کار اشتراکی",
                    Slug = "فضای-کار-اشتراکی-2",
                    IsActive = true,
                    PayAtPlace = true,
                    IconName = "palce.png",
                    CreationDateTime = DateTime.Now,

                },
                new ProductCategory
                {
                    Id = 10,
                    BranchId = 2,
                    CategoryTitle = "کافه",
                    Slug = "کافه-2",
                    IsActive = true,
                    PayAtPlace = true,
                    IconName = "coffe.png",
                    CreationDateTime = DateTime.Now,

                }
            );
        }
    }

}
