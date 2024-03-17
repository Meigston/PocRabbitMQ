using Microsoft.AspNetCore.Mvc;
using PocRabbitMq.Services;

namespace PocRabbitMq.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly RabbitMQPublisher _publisher;

        public PublishController()
        {
            _publisher = new RabbitMQPublisher();
        }

        [HttpPost]
        public IActionResult Publish([FromBody] PublishRequest request)
        {
            _publisher.PublishMessage(request.RoutingKey, request.Message);
            return Ok();
        }
    }

    public class PublishRequest
    {
        public string RoutingKey { get; set; }
        public string Message { get; set; }
    }
}
