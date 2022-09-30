using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IIdentityCartDataService _cartService;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(IIdentityCartDataService cartService, UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartEntry>>> GetProducts()
        {
            return Ok(await _cartService.GetItemsAsync(await CurrentUser()));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddProduct(int id)
        {
            try
            {
                await _cartService.AddProductAsync(id, await CurrentUser());
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
                await _cartService.RemoveProductAsync(id, await CurrentUser());
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
            await _cartService.ClearCartAsync(await CurrentUser());
            return NoContent();
        }

        private async Task<IdentityUser> CurrentUser() => await _userManager.GetUserAsync(User);
    }
}
