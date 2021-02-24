using Microsoft.AspNetCore.Mvc;

namespace AspDotnetCoreApi.Controllers {

    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase {
        public HomeController() {}

        [HttpGet]
        public IActionResult GetHome() {
            return Ok(new {
                message = "Hello from an Asp.Net Core Api"
            });
        }

    }
}