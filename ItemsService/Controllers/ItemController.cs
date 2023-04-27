using ItemsService.Data.Interfaces;
using ItemsService.Helper;
using ItemsService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemsService.Controllers
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : Controller
    {
        private readonly IRabbitMQService _rabbitMQService;

        private readonly IItemRepository _repository;

        public ItemController(IItemRepository repository, IRabbitMQService rabbitMQService)
        {
            _repository = repository;
            _rabbitMQService = rabbitMQService;
        }

        [HttpGet("/get-items")]
        public async Task<IActionResult> GetItems()
        {
            var items = await _repository.GetItems();

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpGet("/get-item/{id}")]
        public async Task<IActionResult> GetSpecificItem(int id)
        {
            var item = await _repository.GetSpecificItem(id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost("/create-item")]
        public async Task<ActionResult<Item>> CreateItem(Item item)
        {
            if (!await _repository.CreateItem(item))
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetSpecificItem), new { id = item.Id }, item);
        }

        [HttpPatch("/update-item/{id}")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            if (!await _repository.ItemExists(id))
            {
                return NotFound();
            }

            await _repository.UpdateItem(item);

            var updatedItem = await _repository.GetSpecificItem(id);

            return Ok(updatedItem);
        }

        [HttpDelete("/delete-item/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            if (!await _repository.ItemExists(id))
            {
                return NotFound();
            }

            var item = await _repository.GetSpecificItem(id);

            await _repository.DeleteItem(item);

            return NoContent();
        }

        [HttpPost("/listen/item-search")]
        public async Task<IActionResult> GetSearchItem()
        {
            var itemForSearch = await _rabbitMQService.ReceiveSearchItem();

            var items = await _repository.SearchItems(itemForSearch);

            _rabbitMQService.SendMessage(items);

            return Ok(items);
        }

    }
}
