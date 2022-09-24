using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartDataService _cartService;

        public CartController(ICartDataService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartEntry>>> GetProducts()
        {
            return Ok(await _cartService.GetItemsAsync());
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddProduct(int id)
        {
            try
            {
                await _cartService.AddProductAsync(id);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            try
            {
                await _cartService.RemoveProductAsync(id);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            return NoContent();
        }
    }
}
