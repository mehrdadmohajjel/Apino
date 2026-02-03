using Apino.Application.Dtos;
using Apino.Application.Dtos.Auth;
using Apino.Application.Services.Auth;
using Apino.Domain.Entities;
using Apino.Domain.Enums;
using Apino.Infrastructure.Data;
using Apino.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            // 1. اعتبارسنجی OTP
            long branchId = 0;
            await _otpService.VerifyAsync(request.Mobile, request.Code);

            // 2. پیدا / ساخت کاربر
            var user = await _db.Users
                .Include(x => x.UserProfile)
                .FirstOrDefaultAsync(x => x.Mobile == request.Mobile);
            
            if (user == null)
            {
                user = new User
                {
                    Mobile = request.Mobile,
                    IsActive = true,
                    CreationDatetime = DateTime.UtcNow,
                    Role = UserRole.User // پیش‌فرض
                };
                _db.Users.Add(user);
                await _db.SaveChangesAsync();
            }
            var _userRole = await _db.BranchUsers
               .FirstOrDefaultAsync(x => x.UserId == user.Id &&
                        x.IsActive &&
                        (x.FinishWorkDate == null || x.FinishWorkDate > DateTime.UtcNow));
            // 🔥 تشخیص Role واقعی
            UserRole finalRole = UserRole.User;

            // SystemAdmin
            if (_userRole== null)
            {
                finalRole = UserRole.User;
            }
            else
            {
                var branchUser = await _db.BranchUsers
                    .Where(x =>
                        x.UserId == user.Id &&
                        x.IsActive &&
                        (x.FinishWorkDate == null || x.FinishWorkDate > DateTime.UtcNow)
                    )
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefaultAsync();

                if (branchUser != null)
                    finalRole = branchUser.Role;
                branchId = Convert.ToInt64(branchUser?.BranchId);
            }
            // 3. JWT (برای Ajax)
            var accessToken = _tokenService.GenerateAccessToken(user);

            // 4. 🔥 COOKIE SIGN-IN (SYNC)
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.MobilePhone, user.Mobile),
        new Claim(ClaimTypes.Role, finalRole.ToString())
    };
                claims.Add(new Claim("BranchId", branchId.ToString()));

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                }
            );

            return Ok(new
            {
                accessToken,
                refreshToken = (string?)null, // اگر نداری
                username = user.Mobile,
                role = finalRole.ToString(),
                profileCompleted = user.UserProfile?.IsProfileCompleted ?? false
            });
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

