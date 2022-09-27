using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trackmeal.Helpers;

namespace Trackmeal.Controllers
{
    [Authorize(Roles = Constants.RoleNames.Administrator)]
    public class ManageController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
