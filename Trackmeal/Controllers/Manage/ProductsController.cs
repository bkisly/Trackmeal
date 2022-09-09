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

        public IActionResult New()
        {
            return View("ProductForm", new Product());
        }

        [HttpGet("{id?}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return View("ProductForm", await _service.GetItemByIdAsync(id));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Product product)
        {
            if (product.Id == 0) await _service.AddItemAsync(product);
            else await _service.EditItemAsync(product.Id, product);

            return RedirectToAction(nameof(Index));
        }
    }
}
