using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Pspcl.Web.Controllers
{
    [Authorize]
    public class AddTipController : Controller
    {
        public IActionResult AddTipView()
        {
            return View();
        }
    }
}
