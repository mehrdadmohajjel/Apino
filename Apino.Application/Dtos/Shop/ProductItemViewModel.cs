using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Shop
{
    public class ProductItemViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; } // تعداد موجودی
        public bool PayAtPlace { get; set; } // آیا پرداخت در محل مجاز است

    }
}
