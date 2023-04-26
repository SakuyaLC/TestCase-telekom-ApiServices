using Microsoft.AspNetCore.Mvc;
using SearchService.Helper;
using SearchService.Model;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : Controller
    {

        private readonly IRabbitMQService _rabbitMQService;

        public SearchController(IRabbitMQService rabbitMQService)
        {
            _rabbitMQService = rabbitMQService;
        }

        [HttpPost("/search-item")]
        public async Task<IActionResult> SearchItem([FromBody] ItemForSearch itemForSearch = null)
        {
            if (itemForSearch == null)
            {
                return BadRequest();
            }

            _rabbitMQService.SendMessage(itemForSearch, "Item");

            var itemsResult = await _rabbitMQService.RecieveItem();

            return Ok(itemsResult);
        }

        [HttpPost("/search-user")]
        public async Task<IActionResult> SearchUser([FromBody] UserForSearch userForSearch = null)
        {
            if (userForSearch == null)
            {
                return BadRequest();
            }

            _rabbitMQService.SendMessage(userForSearch, "User");

            var userResult = await _rabbitMQService.RecieveUser();

            return Ok(userResult);
        }
    }
}
