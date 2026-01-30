using Apino.Application.Dtos;
using Apino.Application.Dtos.Cart;
using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace Apino.Application.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IReadDbContext _db;
        private readonly IToolsService _tools;
        private readonly IConfiguration _config;

        public CartService(IReadDbContext db,IToolsService tools,IConfiguration config)
        {
            _db = db;
            _tools = tools;
            _config = config;
        }

        public async Task<Apino.Domain.Entities.Cart> GetActiveCartAsync(long userId, long branchId)
        {
            return await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.BranchId == branchId);
        }

        public async Task AddAsync(long userId, long branchId, long productId, int qty)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .FirstAsync(p => p.Id == productId);

            var cart = await GetActiveCartAsync(userId, branchId);

            if (cart == null)
            {
                cart = new Domain.Entities.Cart
                {
                    UserId = userId,
                    BranchId = branchId,
                    UpdatedAt = DateTime.UtcNow,
                    OnlyOnlinePayment = false,
                    Items = new List<CartItem>()
                };
                _db.Carts.Add(cart);
            }

            var item = cart.Items.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = productId,
                    Quantity = qty,
                    PayAtPlace = product.Category.PayAtPlace
                });
            }
            else
            {
                item.Quantity += qty;
            }

            // 🔴 قانون طلایی پرداخت
            if (!product.Category.PayAtPlace)
                cart.OnlyOnlinePayment = true;

            cart.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
        }

        public async Task ClearAsync(long cartId)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .FirstAsync(c => c.Id == cartId);

            _db.CartItems.RemoveRange(cart.Items);
            _db.Carts.Remove(cart);

            await _db.SaveChangesAsync();
        }

        public async Task MergeGuestAsync(long userId, List<GuestCartItem> items)
        {
            var branchId = items.First().BranchId;

            // 1️⃣ گرفتن یا ایجاد Draft Order
            var order = await GetOrCreateDraftOrder(userId, branchId);

            bool allowPayAtPlace = true;

            foreach (var item in items)
            {
                var product = await _db.Products
                    .Include(p => p.Category)
                    .FirstAsync(p => p.Id == item.ProductId);

                if (!product.Category.PayAtPlace)
                    allowPayAtPlace = false;

                // Merge با جزئیات موجود یا اضافه کردن جدید
                var detail = order.OrderDetails
                    .FirstOrDefault(x => x.ProductId == item.ProductId);

                if (detail == null)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Qty,
                        Price = product.Price,
                        TotalPrice = product.Price * item.Qty
                    });
                }
                else
                {
                    detail.Quantity += item.Qty;
                    detail.TotalPrice = detail.Price * detail.Quantity;
                }
            }

            // 2️⃣ تعیین نوع پرداخت کل سفارش بر اساس PayAtPlace
            var paymentType = allowPayAtPlace ? PaymentMethod.Cash : PaymentMethod.Online;

            await SetOrderPaymentType(order, paymentType);

            await _db.SaveChangesAsync();
        }

        private async Task<Apino.Domain.Entities.Order> GetOrCreateDraftOrder(long userId, long branchId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Statuses)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.BranchId == branchId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft);

            if (order != null) return order;

            order = new Apino.Domain.Entities.Order
            {
                UserId = userId,
                BranchId = branchId,
                OrderNumber =_tools.GenerateOrderNumber(),
                CreationDate = DateTime.UtcNow,
                CurrentStatusTypeId = (long)PaymentStatus.Draft,
                OrderDetails = new List<OrderDetail>(),
                Statuses = new List<OrderStatus>
        {
            new OrderStatus
            {
                StatusTypeId =(long) PaymentStatus.Draft,
                IsActive = true,
                PaymentTypeId = (long)PaymentMethod.Online, // پیشفرض آنلاین
                CreationDateTime = DateTime.UtcNow
            }
        }
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return order;
        }
        private async Task SetOrderPaymentType(Apino.Domain.Entities.Order order, PaymentMethod paymentMethod)
        {
            // غیرفعال کردن وضعیت‌های قبلی
            foreach (var s in order.Statuses)
                s.IsActive = false;

            order.Statuses.Add(new OrderStatus
            {
                StatusTypeId = (long)PaymentStatus.Draft, // یا وضعیت فعلی مناسب
                IsActive = true,
                PaymentTypeId = (long)paymentMethod,
                CreationDateTime = DateTime.UtcNow
            });

            order.CurrentStatusTypeId = (long)PaymentStatus.Draft;

            await _db.SaveChangesAsync();
        }

 
        public async Task<CartViewModel> GetCartAsync(long userId)
        {
            var order = await _db.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .Where(x => x.UserId == userId && x.CurrentStatusTypeId ==(long) PaymentStatus.Draft)
                .FirstOrDefaultAsync();

            if (order == null)
                return new CartViewModel();

            var items = order.OrderDetails.Select(d => new CartItemVm
            {
                ProductId = d.ProductId,
                Title = d.Product.Title,
                Price = d.Price,
                Quantity = d.Quantity
            }).ToList();

            var sub = items.Sum(x => x.Total);
            var taxPercent = _config.GetValue<int>("Tax:Percent");
            var tax = sub * taxPercent / 100;

            return new CartViewModel
            {
                Items = items,
                SubTotal = sub,
                TaxPercent = tax,
                BranchId = order.BranchId
            };
        }
        public async Task<int> GetCartItemCountAsync(long userId)
        {
            return await _db.CartItems
                .Where(x => x.Cart.UserId == userId)
                .SumAsync(x => x.Quantity);
        }

    }


}
