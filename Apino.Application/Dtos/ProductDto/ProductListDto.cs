using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.ProductDto
{
    public class ProductListDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string CategoryTitle { get; set; }
        public string ServiceTypeTitle { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageName { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        // برای پر کردن فرم ویرایش به این آیدی‌ها نیاز داریم
        public long ProductCategoryId { get; set; }
        public long ServiceTypeId { get; set; }
    }
}
