using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;

namespace Trackmeal.Controllers
{
    public class OrderController : Controller
    {
        // Gets the list of all products with the possibility to add them
        public IActionResult Index()
        {
            return View();
        }

        // Creates a new CartEntry if the product doesn't exist, otherwise increases its amount
        [HttpPost]
        public IActionResult AddProduct(int productId)
        {
            throw new NotImplementedException();
        }

        // Decreases the amount of product in a CartEntry, and removes it if the amount equals 0
        [HttpDelete]
        public IActionResult RemoveProduct(int productId)
        {
            throw new NotImplementedException();
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
