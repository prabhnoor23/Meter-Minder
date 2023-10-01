using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pspcl.Web.Controllers
{
    [Authorize]
    public class IssueTipController : Controller
    {
        public IActionResult IssueTipView()
        {
            return View();
        }
    }
}
