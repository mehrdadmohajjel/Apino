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
}
