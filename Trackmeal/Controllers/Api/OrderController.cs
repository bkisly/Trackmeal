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
        private readonly IOrderDataService _orderService;

        public OrderController(IOrderDataService orderService)
        {
            _orderService = orderService;
        }

        [HttpPut("nextstate/{id}")]
        public async Task<IActionResult> NextState(int id)
        {
            await _orderService.NextStateAsync(id);
            return NoContent();
        }

        [HttpPut("prevstate/{id}")]
        public async Task<IActionResult> PreviousState(int id)
        {
            await _orderService.NextStateAsync(id);
            return NoContent();
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
