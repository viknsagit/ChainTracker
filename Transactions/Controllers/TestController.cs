using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace Transactions.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class TestController : Controller
    {
        CancellationTokenRegistration _cts = new CancellationTokenRegistration();

        [HttpGet("TestRecord")]
        public async Task<IActionResult> TestRecord([FromServices] IProducer<string, string> producer, string msg)
        {

            var kafkaMessage = new Message<string, string>
            {
                Value = msg
            };
            await producer.ProduceAsync("blocks", kafkaMessage, cancellationToken: _cts.Token);
            return Ok();
        }
    }
}
