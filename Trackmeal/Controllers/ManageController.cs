using Microsoft.AspNetCore.Mvc;

namespace Trackmeal.Controllers
{
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
