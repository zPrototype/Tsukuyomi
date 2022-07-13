using Microsoft.AspNetCore.Mvc;

namespace TeamServer.Models
{
    public class HttpListenerController : ControllerBase
    {
        public IActionResult HandleImplant()
        {
            return Ok("Your listener works");
        }
    }
}