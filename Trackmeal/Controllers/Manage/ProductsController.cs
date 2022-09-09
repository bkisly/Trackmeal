using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.Controllers.Manage
{
    [Route("Manage/[controller]/[action]")]
    public class ProductsController : Controller
    {
        private readonly IDataService<Product> _service;

        public ProductsController(IDataService<Product> service)
        {
            _service = service;
        }

        [Route("~/Manage/[controller]")]
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetItemsAsync());
        }

        public string New()
        {
            return "Adding new product";
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                return View(await _service.GetItemByIdAsync(id));
            }
            catch(InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpGet("{id?}")]
        public string Edit(int id)
        {
            return $"Editing product with id {id}";
        }
    }
}
