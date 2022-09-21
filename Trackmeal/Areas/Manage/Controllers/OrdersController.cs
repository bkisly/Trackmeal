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

        public IActionResult Details(int orderId)
        {
            throw new NotImplementedException();
        }
    }
}
