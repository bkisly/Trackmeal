using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;
using Trackmeal.ViewModels;

namespace Trackmeal.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartDataService _service;
        private readonly IModifiableDataService<Product> _productsService;

        public OrderController(ICartDataService service, IModifiableDataService<Product> productsService)
        {
            _service = service;
            _productsService = productsService;
        }

        // Gets the list of all products with the possibility to add them
        public async Task<IActionResult> Index()
        {
            return View(new OrderViewModel
            {
                Products = await _productsService.GetItemsAsync(),
                CartEntries = await _service.GetItemsAsync()
            });
        }

        // Creates a new CartEntry if the product doesn't exist, otherwise increases its amount
        [HttpPost]
        public IActionResult AddProduct(int productId)
        {
            Console.WriteLine($"Adding product of id {productId}");
            return RedirectToAction(nameof(Index));
        }

        // Decreases the amount of product in a CartEntry, and removes it if the amount equals 0
        [HttpPost]
        public IActionResult RemoveProduct(int productId)
        {
            Console.WriteLine($"Removing product of id {productId}");
            return RedirectToAction(nameof(Index));
        }

        // Displays cart and order summary, provides an option to submit an order
        public IActionResult Cart()
        {
            throw new NotImplementedException();
        }

        // Creates a new Order object and adds it to the database
        [HttpPost]
        public IActionResult SubmitOrder()
        {
            throw new NotImplementedException();
        }
    }
}
