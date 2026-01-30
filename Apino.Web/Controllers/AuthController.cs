using Apino.Application.Dtos;
using Apino.Application.Dtos.Auth;
using Apino.Application.Services.Auth;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Apino.Web.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IOtpService _otpService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;
        private readonly AppDbContext _db;

        public AuthController(
            IOtpService otpService,
            ITokenService tokenService,
            AppDbContext db,
            ILogger<AuthController> logger)
        {
            _otpService = otpService;
            _tokenService = tokenService;
            _db = db;
            _logger = logger;
        }

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req.Mobile))
                    return BadRequest(new { message = "شماره موبایل الزامی است" });

                await _otpService.SendAsync(req.Mobile);
                return Ok(new { message = "کد تایید ارسال شد" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ارسال OTP به {Mobile}", req.Mobile);
                return StatusCode(500, new { message = "ارسال کد با خطا مواجه شد. دوباره تلاش کنید" });
            }
        }


        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            try
            {
                // بررسی OTP
                await _otpService.VerifyAsync(request.Mobile, request.Code);

                // بررسی اینکه کاربر وجود دارد یا نه
                var user = _db.Users.FirstOrDefault(u => u.Mobile == request.Mobile);
                if (user == null)
                {
                    // Guest → Auto Register
                    user = new User
                    {
                        Mobile = request.Mobile,
                        IsActive = true,
                        Role = UserRole.User,
                        CreationDatetime = DateTime.UtcNow
                    };
                    _db.Users.Add(user);
                    await _db.SaveChangesAsync();

                    // ساخت پروفایل خالی
                    _db.UserProfiles.Add(new UserProfile
                    {
                        UserId = user.Id,
                        IsProfileCompleted = false,
                        CreationDateTime = DateTime.UtcNow
                    });
                    await _db.SaveChangesAsync();
                }

                // JWT + Refresh Token
                var accessToken = _tokenService.GenerateAccessToken(user);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // ذخیره Refresh Token در DB
                _db.UserTokens.Add(new UserToken
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    ExpireAt = DateTime.UtcNow.AddDays(30),
                    CreationDateTime = DateTime.UtcNow,
                    IsRevoked = false,
                    RevokedAt = null
                });
                await _db.SaveChangesAsync();
                // 🟢 Merge Guest Cart
                //var guestItemsJson = request.GuestCartJson; // JSON array [{productId, branchId, qty, payAtPlace}]
                //if (!string.IsNullOrEmpty(guestItemsJson))
                //{
                //    var guestItems = JsonSerializer.Deserialize<List<GuestCartItem>>(guestItemsJson);
                //    if (guestItems != null && guestItems.Count > 0)
                //    {
                //        foreach (var item in guestItems)
                //        {
                //            await _cartService.AddAsync(user.Id, item.BranchId, item.ProductId, item.Qty);
                //        }
                //    }
                //}
                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    profileCompleted = user.UserProfile?.IsProfileCompleted ?? false
                });
            }
            catch (Exception ex)
            {
                // می‌توانید اینجا لاگ هم کنید
                _logger.LogError(ex, "خطا در VerifyOtp");

                return BadRequest(new { message = "عملیات ناموفق بود، لطفا دوباره تلاش کنید" });
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return BadRequest("RefreshToken ارسال نشده");

            var token = await _db.UserTokens
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == request.RefreshToken &&
                    !x.IsRevoked);

            if (token != null)
            {
                token.IsRevoked = true;
                token.RevokedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }

            return Ok();
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
        {
            var token = await _db.UserTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x =>
                    x.RefreshToken == request.RefreshToken &&
                    !x.IsRevoked &&
                    x.ExpireAt > DateTime.UtcNow);

            if (token == null)
                return Unauthorized();

            // revoke قبلی
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;

            // توکن جدید
            var newRefreshToken = Guid.NewGuid().ToString("N");

            await _db.UserTokens.AddAsync(new UserToken
            {
                UserId = token.UserId,
                RefreshToken = newRefreshToken,
                CreationDateTime = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddDays(30),
                IsRevoked = false
            });

            var newAccessToken = _tokenService.GenerateAccessToken(token.User);

            await _db.SaveChangesAsync();

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken
            });
        }

    }
    public record VerifyOtpRequest(string Mobile, string Code);
}

