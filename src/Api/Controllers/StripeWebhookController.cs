using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using System.IO;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/v1/stripe-webhook")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IWebhookHandlerService _webhookHandler;
        private readonly ILogger<StripeWebhookController> _logger;

        public StripeWebhookController(IWebhookHandlerService webhookHandler, ILogger<StripeWebhookController> logger)
        {
            _webhookHandler = webhookHandler;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];

            try
            {
                await _webhookHandler.ProcessWebhookEventAsync(json, signature!);
                return Ok();
            }
            catch (StripeException e)
            {
                _logger.LogWarning(e, "Invalid Stripe webhook signature received.");
                return BadRequest(new { error = "Invalid webhook signature." });
            }
        }
    }
}