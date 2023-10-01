using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pspcl.Core.Domain;

namespace Pspcl.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        //Dashboard View Binding
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.FirstName = user.FirstName.ToUpper();
                _logger.LogInformation($"Logged In Success");
                ViewBag.Message = TempData["Message"];
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing your request: {ErrorMessage}", ex.Message);
                return View("Error");
            }
           
        }
    }
}
