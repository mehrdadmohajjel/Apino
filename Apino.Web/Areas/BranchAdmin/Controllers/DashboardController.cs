using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{

        [Area("BranchAdmin")]
        public class DashboardController : BranchAdminBaseController
    {
            public IActionResult Index()
            {
                return View();
            }
        }
}
