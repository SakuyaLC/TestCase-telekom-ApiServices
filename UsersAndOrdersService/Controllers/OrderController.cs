using Microsoft.AspNetCore.Mvc;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(DataContext context)
        {
            _orderRepository = new OrderRepository(context);
        }

        [HttpGet("/get-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetOrders();
            return Ok(orders);
        }

        [HttpGet("/get-order/{id}")]
        public async Task<IActionResult> GetSpecificOrder(int id)
        {
            var order = await _orderRepository.GetSpecificOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpPost("/create-order")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await _orderRepository.CreateOrder(order);

            return CreatedAtAction(nameof(GetSpecificOrder), new { id = order.Id }, order);
        }

        [HttpPatch("/update-order/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            if (!await _orderRepository.OrderExists(id))
            {
                return NotFound();
            }

            var updatedOrder = await _orderRepository.GetSpecificOrder(id);

            return Ok(updatedOrder);
        }

        [HttpDelete("/delete-order/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!await _orderRepository.OrderExists(id))
            {
                return NotFound();
            }

            var order = await _orderRepository.GetSpecificOrder(id);

            await _orderRepository.DeleteOrder(order);

            return NoContent();
        }
    }
}
