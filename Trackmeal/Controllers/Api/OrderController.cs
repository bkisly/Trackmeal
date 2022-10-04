using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trackmeal.Helpers;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Constants.RoleNames.Administrator)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderDataService _orderService;

        public OrderController(IOrderDataService orderService)
        {
            _orderService = orderService;
        }

        [HttpPut("nextstate/{id}")]
        public async Task<ActionResult<OrderStatus>> NextState(int id)
        {
            await _orderService.NextStateAsync(id);
            return Ok((await _orderService.GetItemByIdAsync(id)).OrderStatus);
        }

        [HttpPut("prevstate/{id}")]
        public async Task<ActionResult<OrderStatus>> PreviousState(int id)
        {
            await _orderService.PreviousStateAsync(id);
            return Ok((await _orderService.GetItemByIdAsync(id)).OrderStatus);
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
