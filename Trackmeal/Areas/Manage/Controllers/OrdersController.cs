using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class OrdersController : Controller
    {
        private readonly IModifiableDataService<Order> _orderService;

        public OrdersController(IModifiableDataService<Order> orderService)
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
