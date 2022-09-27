using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;
using Trackmeal.ViewModels;

namespace Trackmeal.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartDataService _cartService;
        private readonly IModifiableDataService<Product> _productsService;
        private readonly IOrderDataService _orderService;

        public OrderController(ICartDataService service, 
            IModifiableDataService<Product> productsService, IOrderDataService orderService)
        {
            _cartService = service;
            _productsService = productsService;
            _orderService = orderService;
        }

        // Gets the list of all products with the possibility to add them
        public async Task<IActionResult> Index()
        {
            return View(new OrderViewModel
            {
                Products = await _productsService.GetItemsAsync(),
                CartEntries = await _cartService.GetItemsAsync()
            });
        }

        // Displays cart and order summary, provides an option to submit an order
        public async Task<IActionResult> Cart()
        {
            return View(await _cartService.GetItemsAsync());
        }

        // Creates a new Order object and adds it to the database
        [HttpPost]
        public async Task<IActionResult> Submit()
        {
            var order = new Order { Entries = (await _cartService.GetItemsAsync()).ToList() };
            await _orderService.AddItemAsync(order);

            return RedirectToAction(nameof(Summary), new { id = order.Id });
        }

        // Shows the summary of an order with given ID
        public async Task<IActionResult> Summary(int id)
        {
            try
            {
                return View(await _orderService.GetItemByIdAsync(id));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        // Displays status tracking page for the order with given token
        [AllowAnonymous]
        public async Task<IActionResult> Status(Guid token)
        {
            try
            {
                return View(await _orderService.GetOrderByTokenAsync(token));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
    }
}
