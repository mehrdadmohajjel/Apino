using Apino.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Controllers
{
    namespace Apino.Web.Controllers
    {
        [Route("profile")]
        public class ProfileController : Controller
        {
            private readonly AppDbContext _db;

            public ProfileController(AppDbContext db)
            {
                _db = db;
            }

            [HttpGet("complete")]
            public IActionResult Complete()
            {
                return View();
            }

            [HttpPost("complete")]
            public async Task<IActionResult> Complete([FromForm] ProfileCompleteRequest model)
            {
                var userId = GetCurrentUserId(); // از JWT استخراج شود
                var profile = _db.UserProfiles.FirstOrDefault(p => p.UserId == userId);
                if (profile == null) return BadRequest();

                profile.FullName = model.FullName;
                profile.Email = model.Email;
                profile.TelegramId = model.TelegramId;
                profile.InstagramUserName = model.InstagramUserName;
                profile.LinkedInAddress = model.LinkedInAddress;
                profile.IsProfileCompleted = true;

                await _db.SaveChangesAsync();
                return Redirect("/cart");
            }

            private long GetCurrentUserId()
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
                return long.Parse(userIdClaim!.Value);
            }
        }

        public record ProfileCompleteRequest(
            string FullName,
            string Email,
            string TelegramId,
            string InstagramUserName,
            string LinkedInAddress
        );
    }
}
