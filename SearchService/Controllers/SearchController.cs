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

        [HttpPost("/get-items")]
        public IActionResult SearchItem(ItemForSearch itemForSearch)
        {
            _rabbitMQService.SendMessage("get-items");
            return Ok();
        }
    }
}
