using Apino.Domain.Entities;
using System.Security.Claims;

public static class ClaimsExtensions
{
    public static long GetBranchId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst("BranchId");
        if (claim == null || string.IsNullOrEmpty(claim.Value))
        {
            // یا خطا برگردانید یا صفر، بسته به لاجیک برنامه
            throw new UnauthorizedAccessException("شناسه شعبه در توکن کاربر یافت نشد.");
        }

        return long.Parse(claim.Value);
    }
    public static long GetUserID(this ClaimsPrincipal user)
    {
        // بهترین روش، استفاده از کلیم استاندارد NameIdentifier برای شناسه کاربر است
        var claim = user.FindFirst(ClaimTypes.NameIdentifier);

        if (claim == null || string.IsNullOrEmpty(claim.Value))
        {
            // پرتاب خطا با پیام صحیح
            throw new UnauthorizedAccessException("شناسه کاربر (UserID) در توکن یافت نشد.");
        }

        if (long.TryParse(claim.Value, out long userId))
        {
            return userId;
        }

        // اگر مقدار داخل کلیم عددی نباشد، خطا می‌دهیم
        throw new InvalidOperationException("قالب شناسه کاربر در توکن نامعتبر است.");
    }
}
