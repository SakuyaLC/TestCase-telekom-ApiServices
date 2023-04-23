using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly OrderRepository _orderRepository;

        public OrderController(DataContext context)
        {
            _orderRepository = new OrderRepository(context);
        }

        [Authorize(Roles = "user, admin")]
        [HttpGet("/get-orders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetOrders();
            return Ok(orders);
        }

        [Authorize(Roles = "user, admin")]
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

        [Authorize(Roles = "user, admin")]
        [HttpPost("/create-order")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            await _orderRepository.CreateOrder(order);

            return CreatedAtAction(nameof(GetSpecificOrder), new { id = order.Id }, order);
        }

        [Authorize(Roles = "user, admin")]
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

        [Authorize(Roles = "user, admin")]
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
