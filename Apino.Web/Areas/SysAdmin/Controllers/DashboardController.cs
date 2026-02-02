using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.SysAdmin.Controllers
{

    [Area("SysAdmin")]
    [Authorize(Roles = "SysAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
