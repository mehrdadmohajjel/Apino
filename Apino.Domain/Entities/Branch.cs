using Apino.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Address { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<ProductCategory> Categories { get; set; }
    }

}
