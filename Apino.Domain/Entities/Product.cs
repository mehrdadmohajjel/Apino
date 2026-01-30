using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class Product : BaseEntity
    {
        public long ProductCategoryId { get; set; }
        public long ServiceTypeId { get; set; }
        public long BranchId { get; set; }
        public string Title { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageName { get; set; } = null!;
        public string Description { get; set; } = null!;
        [ConcurrencyCheck]
        public int Stock { get; set; }
        public bool IsActive { get; set; }


        public ProductCategory Category { get; set; } = null!;
        public ServiceType ServiceType { get; set; } = null!;
    }

}
