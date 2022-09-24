using Microsoft.AspNetCore.Mvc;
using Trackmeal.Services;

namespace Trackmeal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class OrdersController : Controller
    {
        private readonly IOrderDataService _orderService;

        public OrdersController(IOrderDataService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _orderService.GetItemsAsync());
        }

        public async Task<IActionResult> Details(int orderId)
        {
            return View(await _orderService.GetItemByIdAsync(orderId));
        }
    }
}
