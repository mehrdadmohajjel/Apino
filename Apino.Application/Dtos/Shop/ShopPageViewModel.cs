using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Shop
{
    public class ShopPageViewModel
    {
        public long BranchId { get; set; }
        public long? CategoryId { get; set; }

        public List<CategoryMenuViewModel> Categories { get; set; }
        public List<ProductItemViewModel> Products { get; set; }

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

}
