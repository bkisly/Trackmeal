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
        public async Task<IActionResult> AddProduct(int id)
        {
            Console.WriteLine($"Adding product of id {id}");

            try
            {
                await _service.AddProductAsync(id);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Decreases the amount of product in a CartEntry, and removes it if the amount equals 0
        [HttpDelete]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            Console.WriteLine($"Removing product of id {id}");

            try
            {
                await _service.RemoveProductAsync(id);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // Displays cart and order summary, provides an option to submit an order
        public async Task<IActionResult> Cart()
        {
            return View(new CartViewModel { CartEntries = await _service.GetItemsAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _service.ClearCartAsync();
            return NoContent();
        }

        // Creates a new Order object and adds it to the database
        [HttpPost]
        public IActionResult SubmitOrder()
        {
            throw new NotImplementedException();
        }
    }
}
