using Apino.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apino.Application.Dtos.Cart
{ 
    public class CartViewModel
    {
        public List<CartItemVm> Items { get; set; } = new();
        public decimal SubTotal { get; set; }
        public decimal TaxPercent { get; set; }

        public decimal TaxAmount =>
            Math.Round(SubTotal * TaxPercent / 100, 0);
        public decimal GrandTotal =>
            SubTotal + TaxAmount;
        public long BranchId { get; set; }
    }
}
