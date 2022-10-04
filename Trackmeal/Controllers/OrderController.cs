using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;
using Trackmeal.ViewModels;

namespace Trackmeal.Controllers
{
    public class OrderController : Controller
    {
        private readonly IIdentityCartDataService _cartService;
        private readonly IModifiableDataService<Product> _productsService;
        private readonly IOrderDataService _orderService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrderController(IIdentityCartDataService cartService, 
            IModifiableDataService<Product> productsService, IOrderDataService orderService,
            UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            _productsService = productsService;
            _orderService = orderService;
            _userManager = userManager;
        }

        // Gets the list of all products with the possibility to add them
        public async Task<IActionResult> Index()
        {
            return View(new OrderViewModel
            {
                Products = await _productsService.GetItemsAsync(),
                CartEntries = await _cartService.GetItemsAsync(await CurrentUser())
            });
        }

        // Displays cart and order summary, provides an option to submit an order
        public async Task<IActionResult> Cart()
        {
            return View(await _cartService.GetItemsAsync(await CurrentUser()));
        }

        // Creates a new Order object and adds it to the database
        [HttpPost]
        public async Task<IActionResult> Submit()
        {
            var order = new Order { Entries = (await _cartService.GetItemsAsync(await CurrentUser())).ToList() };
            await _orderService.AddItemAsync(order, await CurrentUser());

            return RedirectToAction(nameof(Summary), new { id = order.Id });
        }

        // Shows the summary of an order with given ID
        public async Task<IActionResult> Summary(int id)
        {
            try
            {
                return View(await _orderService.GetItemByIdAsync(id, await CurrentUser()));
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

        private async Task<IdentityUser> CurrentUser() => await _userManager.GetUserAsync(User);
    }
}
