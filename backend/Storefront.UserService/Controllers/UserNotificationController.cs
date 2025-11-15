using Microsoft.AspNetCore.Mvc;
using Storefront.UserService.Models.Request;
using Storefront.UserService.Services;

namespace Storefront.UserService.Controllers
{
    [ApiController]
    [Route("Notification")]
    public class UserNotificationController: ControllerBase
    {
        private readonly ILogger<UserNotificationController> _logger;
        private readonly INotificationService _notificationService;

        public UserNotificationController(ILogger<UserNotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpPost("Contact")]
        public async Task<ActionResult> Contact([FromBody] ContactRequest request) {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _notificationService.SendContactEmail(request);
            return NoContent();
        }
    }
}
