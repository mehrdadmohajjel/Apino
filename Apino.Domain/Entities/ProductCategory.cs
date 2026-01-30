using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class ProductCategory : BaseEntity
    {
        public long BranchId { get; set; }
        public string CategoryTitle { get; set; }
        public string Slug { get; set; }
        public bool IsActive { get; set; }
        public bool PayAtPlace { get; set; }
        public string IconName { get; set; }
        public DateTime CreationDateTime { get; set; }

        public Branch Branch { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

}
