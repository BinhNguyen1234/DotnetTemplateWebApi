using Microsoft.AspNetCore.Mvc;

namespace RateLimiterService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckController
        : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var a = await Task.FromResult("Hello from RateLimiterService");
            return Ok(a);
        }

        [HttpGet]
        public async Task<IActionResult> GetCached()
        {
            var a = await Task.FromResult("Hello from RateLimiterService with delay");
            return Ok(a);
        }

        [HttpPost]
        public async Task<IActionResult> SetCached()
        {
            var a = await Task.FromResult("Hello from RateLimiterService");
            return Ok(a);
        }
    }
}
