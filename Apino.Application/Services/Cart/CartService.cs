using Apino.Application.Common.Exceptions;
using Apino.Application.Dtos;
using Apino.Application.Dtos.Cart;
using Apino.Application.Interfaces;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Linq;

namespace Apino.Application.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IReadDbContext _db;
        private readonly IToolsService _tools;
        private readonly IConfiguration _config;

        public CartService(
            IReadDbContext db,
            IToolsService tools,
            IConfiguration config)
        {
            _db = db;
            _tools = tools;
            _config = config;
        }

        // =========================================================
        // 🛒 Add To Cart
        // =========================================================
        public async Task AddAsync(
            long userId,
            long branchId,
            long productId,
            int qty,
            decimal price
        )
        {
            if (qty <= 0)
                throw new ArgumentException("Quantity نامعتبر");

            using var tx = await (_db as DbContext)!.Database.BeginTransactionAsync();

            // 🔒 گرفتن Draft با قفل
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .Where(o =>
                    o.UserId == userId &&
                    o.BranchId == branchId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                )
                .FirstOrDefaultAsync();

            if (order == null)
            {
                order = new Domain.Entities.Order
                {
                    UserId = userId,
                    BranchId = branchId,
                    OrderNumber = _tools.GenerateOrderNumber(),
                    CreationDate = DateTime.UtcNow,
                    CurrentStatusTypeId = (long)PaymentStatus.Draft,
                    OrderDetails = new List<OrderDetail>()
                };

                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
            }

            var item = order.OrderDetails.FirstOrDefault(x => x.ProductId == productId);

            if (item == null)
            {
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = productId,
                    Quantity = qty,
                    Price = price,              // 🔄 sync قیمت
                    TotalPrice = price * qty
                });
            }
            else
            {
                item.Quantity += qty;
                item.TotalPrice = item.Quantity * item.Price;
            }

            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }



        // =========================================================
        // ➕ / ➖ Update Quantity
        // =========================================================
        public async Task UpdateQuantityAsync(long userId, long productId, int quantity)
        {
            if (quantity < 0)
                throw new Exception("تعداد نامعتبر");

            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order == null)
                return;

            var item = order.OrderDetails.FirstOrDefault(x => x.ProductId == productId);
            if (item == null)
                return;

            if (quantity == 0)
                _db.OrderDetails.Remove(item);
            else
            {
                item.Quantity = quantity;
                item.TotalPrice = item.Price * quantity;
            }

            await _db.SaveChangesAsync();
        }

        // =========================================================
        // ❌ Remove Item
        // =========================================================
        public async Task RemoveAsync(long userId, long productId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order == null) return;

            var item = order.OrderDetails.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return;

            _db.OrderDetails.Remove(item);
            await _db.SaveChangesAsync();
        }


        // =========================================================
        // 🔢 Cart Count (Badge)
        // =========================================================
        public async Task<int> GetCartItemCountAsync(long userId)
        {
            return await _db.Orders
                .Where(o =>
                    o.UserId == userId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                )
                .SelectMany(o => o.OrderDetails)
                .SumAsync(x => (int?)x.Quantity) ?? 0;
        }

        public async Task<int> GetCartCountAsync(long userId)
        {
            return await GetCartItemCountAsync(userId);
        }

        // =========================================================
        // 📦 Get Cart (View)
        // =========================================================
        public async Task<CartViewModel> GetCartAsync(long userId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order == null)
                return new CartViewModel();

            var items = order.OrderDetails.Select(d => new CartItemVm
            {
                ProductId = d.ProductId,
                Title = d.Product.Title,
                Price = d.Price,
                Quantity = d.Quantity,
                ImageName = d.Product.ImageName
            }).ToList();

            var subTotal = items.Sum(x => x.Total);
            var taxPercent = _config.GetValue<int>("Tax:Percent");
            var taxAmount = subTotal * taxPercent / 100;

            return new CartViewModel
            {
                Items = items,
                SubTotal = subTotal,
                TaxPercent = taxPercent,
                BranchId = order.BranchId
            };
        }

        // =========================================================
        // 🔄 Merge Guest Cart
        // =========================================================
        public async Task MergeGuestAsync(long userId, List<GuestCartItem> items)
        {
            if (items == null || items.Count == 0)
                return;

            var branchId = items.First().BranchId;
            var order = await GetOrCreateDraftOrder(userId, branchId);

            var productIds = items.Select(x => x.ProductId).Distinct().ToList();

            var products = await _db.Products
                .Where(x => productIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            foreach (var guestItem in items)
            {
                if (!products.TryGetValue(guestItem.ProductId, out var product))
                    continue;

                var qty = Math.Max(guestItem.Qty, 1);

                var detail = order.OrderDetails
                    .FirstOrDefault(x => x.ProductId == product.Id);

                if (detail == null)
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = product.Id,
                        Quantity = qty,
                        Price = product.Price,
                        TotalPrice = product.Price * qty
                    });
                }
                else
                {
                    detail.Quantity += qty;
                    detail.TotalPrice = detail.Quantity * detail.Price;
                }
            }

            await _db.SaveChangesAsync();
        }

        // =========================================================
        // 🧹 Clear Cart (Draft Order)
        // =========================================================
        public async Task ClearAsync(long orderId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o =>
                    o.Id == orderId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order == null) return;

            _db.OrderDetails.RemoveRange(order.OrderDetails);
            _db.Orders.Remove(order);

            await _db.SaveChangesAsync();
        }

        // =========================================================
        // 🔧 Helpers
        // =========================================================
        private async Task<Domain.Entities.Order> GetDraftOrder(long userId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order == null)
                throw new Exception("سبد خرید یافت نشد");

            return order;
        }

        private async Task<Domain.Entities.Order> GetOrCreateDraftOrder(long userId, long branchId)
        {
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Statuses)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.BranchId == branchId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );

            if (order != null) return order;

            order = new Domain.Entities.Order
            {
                UserId = userId,
                BranchId = branchId,
                OrderNumber = _tools.GenerateOrderNumber(),
                CreationDate = DateTime.UtcNow,
                CurrentStatusTypeId = (long)PaymentStatus.Draft,
                OrderDetails = new List<OrderDetail>(),
                Statuses = new List<OrderStatus>
            {
                new OrderStatus
                {
                    StatusTypeId = (long)PaymentStatus.Draft,
                    PaymentTypeId = (long)PaymentMethod.Online,
                    IsActive = true,
                    CreationDateTime = DateTime.UtcNow
                }
            }
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return order;
        }

        public async Task<Domain.Entities.Order> GetActiveCartAsync(long userId, long branchId)
        {
            return await _db.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.BranchId == branchId &&
                    o.CurrentStatusTypeId == (long)PaymentStatus.Draft
                );
        }
    }

}
