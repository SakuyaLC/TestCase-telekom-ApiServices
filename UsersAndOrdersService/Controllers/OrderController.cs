using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.DTO;
using UsersAndOrdersService.Data.Interfaces;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Helper;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderedItemsRepository _orderedItemsRepository;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IMapper _mapper;

        public OrderController(DataContext context, IOrderRepository orderRepository, IOrderedItemsRepository orderedItemsRepository, IRabbitMQService rabbitMQService, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderedItemsRepository = orderedItemsRepository;
            _rabbitMQService = rabbitMQService;
            _mapper = mapper;

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

        [HttpGet("/get-orderInfo")]
        public async Task<IActionResult> GetOrderInfo(int id)
        {
            if (!await _orderRepository.OrderExists(id))
            {
                return NotFound();
            }

            var order = await _orderRepository.GetSpecificOrder(id);
            order.OrderedItems = await _orderedItemsRepository.GetOrderedItemsForOrder(order.Id);

            return Ok(_mapper.Map<OrderInfoDTO>(order));
        }
    }
}
