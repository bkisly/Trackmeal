using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IModifiableDataService<Order> _orderService;

        public OrderController(IModifiableDataService<Order> orderService)
        {
            _orderService = orderService;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _orderService.DeleteItemAsync(id);
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
