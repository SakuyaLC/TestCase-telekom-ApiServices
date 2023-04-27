using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UsersAndOrdersService.Data.Context;
using UsersAndOrdersService.Data.DTO;
using UsersAndOrdersService.Data.Interfaces;
using UsersAndOrdersService.Data.Repositories;
using UsersAndOrdersService.Model;

namespace UsersAndOrdersService.Controllers
{
    [ApiController]
    [Route("api/ordered-items")]
    public class OrderedItemsController : Controller
    {
        private readonly IOrderedItemsRepository _orderedItemsRepository;
        private readonly IMapper _mapper;

        public OrderedItemsController(IOrderedItemsRepository orderedItemsRepository, IMapper mapper)
        {
            _orderedItemsRepository = orderedItemsRepository;
            _mapper = mapper;
        }

        //[Authorize(Roles = "user, admin")]
        [HttpGet("/get-orderedItems")]
        public async Task<IActionResult> GetOrderedItems()
        {
            var orderedItems = _mapper.Map<List<OrderedItemDTO>>(await _orderedItemsRepository.GetOrderedItems());
            return Ok(orderedItems);
        }

        //[Authorize(Roles = "user, admin")]
        [HttpGet("/get-orderedItem/{id}")]
        public async Task<IActionResult> GetSpecificOrderedItem(int id)
        {
            var orderedItem = _mapper.Map<OrderedItemDTO>(await _orderedItemsRepository.GetSpecificOrderedItem(id));

            if (orderedItem == null)
            {
                return NotFound();
            }

            return Ok(orderedItem);
        }

        //[Authorize(Roles = "user, admin")]
        [HttpPost("/create-orderedItem")]
        public async Task<IActionResult> CreateOrderedItem(OrderedItemDTO orderedItemDTO)
        {
            OrderedItem orderedItem = _mapper.Map<OrderedItem>(orderedItemDTO);
            await _orderedItemsRepository.CreateOrderedItem(orderedItem);
            var orderedItemDto = _mapper.Map<OrderedItemDTO>(CreatedAtAction(nameof(GetSpecificOrderedItem), new { id = orderedItem.Id }, orderedItem));
            return Ok(orderedItemDto);
        }

        //[Authorize(Roles = "user, admin")]
        [HttpPatch("/update-orderedItem/{id}")]
        public async Task<IActionResult> UpdateOrderedItem(int id, OrderedItemDTO orderedItem)
        {
            if (id != orderedItem.Id)
            {
                return BadRequest();
            }

            if (!await _orderedItemsRepository.OrderedItemExists(id))
            {
                return NotFound();
            }

            var updatedOrderedItem = _mapper.Map<OrderedItemDTO>(await _orderedItemsRepository.GetSpecificOrderedItem(id));

            return Ok(updatedOrderedItem);
        }

        //[Authorize(Roles = "user, admin")]
        [HttpDelete("/delete-orderedItem/{id}")]
        public async Task<IActionResult> DeleteOrderedItem(int id)
        {
            if (!await _orderedItemsRepository.OrderedItemExists(id))
            {
                return NotFound();
            }

            var orderedItem = await _orderedItemsRepository.GetSpecificOrderedItem(id);

            await _orderedItemsRepository.DeleteOrderedItem(orderedItem);

            return NoContent();
        }
    }
}
