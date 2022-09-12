using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Trackmeal.Models;
using Trackmeal.Services;
using Trackmeal.ViewModels;

namespace Trackmeal.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProductsController : Controller
    {
        private readonly IModifiableDataService<Product> _service;

        public ProductsController(IModifiableDataService<Product> service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _service.GetItemsAsync());
        }

        public IActionResult New()
        {
            return View("ProductForm", new ProductFormViewModel());
        }

        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                return View("ProductForm", new ProductFormViewModel(await _service.GetItemByIdAsync(id)));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Product product)
        {
            if (!ModelState.IsValid)
                return View("ProductForm", new ProductFormViewModel(product));

            if (product.Id == 0) await _service.AddItemAsync(product);
            else await _service.EditItemAsync(product.Id, product);

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteItemAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
