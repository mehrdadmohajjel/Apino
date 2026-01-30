using Apino.Application.Dtos;
using Apino.Application.Dtos.Cart;
using Apino.Application.Services.Cart;
using Apino.Application.Services.Order;
using Apino.Domain.Entities;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Apino.Web.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IConfiguration _config;

        public CartController(ICartService cartService,IOrderService orderService, IConfiguration config)
        {
            _cartService = cartService;
            _orderService = orderService;
            _config = config;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody] AddToCartRequest req)
        {
            var userId = long.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")!.Value
            );

            await _cartService.AddAsync(
                userId,
                req.BranchId,
                req.ProductId,
                1
            );

            var count = await _cartService.GetCartItemCountAsync(userId);

            return Ok(new { count });
        }
        [Authorize]
        [HttpGet("count")]
        public async Task<IActionResult> Count()
        {
            var userId = long.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")!.Value
            );

            var count = await _cartService.GetCartItemCountAsync(userId);

            return Ok(new { count });
        }


        // ===============================
        // Merge Guest Cart After Login
        // ===============================
        [Authorize]
        [HttpPost("merge")]
        public async Task<IActionResult> Merge([FromBody] List<GuestCartItem> items)
        {
            if (items == null || items.Count == 0)
                return Ok();

            var userId = long.Parse(User.FindFirst("userId")!.Value);

            // Merge واقعی
            await _cartService.MergeGuestAsync(userId, items);

            return Ok();
        }


        // ===============================
        // Cart Page
        // ===============================
        [Authorize]
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var userId = long.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")!.Value
            );

            var cart = await _cartService.GetCartAsync(userId);

            var taxPercent = _config.GetValue<int>("Tax:Percent");

            var subTotal = cart.Items.Sum(x => x.Price * x.Quantity);
            var taxAmount = subTotal * taxPercent / 100;

            var model = new CartViewModel
            {
                BranchId = cart.BranchId,
                Items = cart.Items.Select(x => new CartItemVm
                {
                    ProductId = x.ProductId,
                    Title = x.Title,
                    Quantity = x.Quantity,
                    Price = x.Price,
                    ImageName = x.ImageName
                }).ToList(),

                SubTotal = subTotal,
                TaxPercent = taxPercent,
                
            };

            return View(model);
        }

       

        [Authorize]
        [HttpGet("checkout")]
        public async Task<IActionResult> Checkout(long branchId)
        {
            var userId = long.Parse(User.FindFirst("userId")!.Value);

            var orderId = await _orderService.CreateFromCartAsync(userId, branchId);

            return RedirectToAction("Pay", "Payment", new { orderId });
        }
    }

    public record AddToCartRequest(long ProductId, bool PayAtPlace ,long BranchId);
}
