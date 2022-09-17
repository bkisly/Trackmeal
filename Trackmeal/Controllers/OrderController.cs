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

        public OrderController(ICartDataService service, IModifiableDataService<Product> productsService)
        {
            _cartService = service;
            _productsService = productsService;
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
            return View(new CartViewModel { CartEntries = await _cartService.GetItemsAsync() });
        }

        // Creates a new Order object and adds it to the database
        [HttpPost]
        public IActionResult SubmitOrder()
        {
            throw new NotImplementedException();
        }
    }
}
