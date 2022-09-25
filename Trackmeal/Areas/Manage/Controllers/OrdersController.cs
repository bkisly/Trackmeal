using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
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
            return View((await _orderService.GetItemsAsync()).Where(order => 
                order.OrderStatus.Id != (byte)OrderStatusEnum.Completed));
        }

        public async Task<IActionResult> Details(int orderId)
        {
            try
            {
                return View(await _orderService.GetItemByIdAsync(orderId));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> Completed()
        {
            return View((await _orderService.GetItemsAsync()).Where(order =>
                order.OrderStatus.Id == (byte)OrderStatusEnum.Completed));
        }
    }
}
