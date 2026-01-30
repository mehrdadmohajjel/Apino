using Apino.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Infrastructure.Data.Seeds
{
    public static class ServiceTypeSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // نمونه کارمندان شعبه
            modelBuilder.Entity<ServiceType>().HasData(
                new ServiceType
                {
                    Id = 1,
                    ServiceTypeTitle = "جلسه ای",
                    ServiceTypeEnglishName = "CountByUse",
                    IsSessionBased = true,

                },
     new ServiceType
     {
         Id = 2,
         ServiceTypeTitle = "ماهانه",
         ServiceTypeEnglishName = "Monthly",
         IsSessionBased = false,

     }
            );
        }

    }
}
