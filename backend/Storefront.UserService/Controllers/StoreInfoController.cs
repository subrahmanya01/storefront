using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Storefront.UserService.Models.Common;

namespace Storefront.UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StoreInfoController : ControllerBase
    {
        private readonly IOptions<StoreInfo> _storeInfo;
        public StoreInfoController(IOptions<StoreInfo> storeInfo)
        {
            _storeInfo = storeInfo;
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok(_storeInfo.Value);
        }
    }
}
