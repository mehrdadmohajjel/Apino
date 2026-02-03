using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Apino.Web.Areas.BranchAdmin.Controllers
{
    [Area("BranchAdmin")]
    [Authorize(Roles = "BranchAdmin")]
    public class BranchAdminBaseController : Controller
    {
    }
}
