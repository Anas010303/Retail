using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ABC_Retail.Controllers
{
    public class OrderController : Controller
    {
        private readonly StorageService _storageService;

        public OrderController(StorageService storageService)
        {
            _storageService = storageService;
        }

        // Add message to the queue
        [HttpPost]
        public async Task<IActionResult> AddMessageToQueue(string queueName, string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            await _storageService.AddMessageToQueueAsync(queueName, message);
            return Ok("Message added to queue.");
        }
    }
}
