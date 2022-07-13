using ApiModels.Requests;
using Microsoft.AspNetCore.Mvc;
using TeamServer.Services;
using HttpListener = TeamServer.Models.HttpListener;

namespace TeamServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListenerController : ControllerBase
    {
        private readonly IListenerService _listeners;

        public ListenerController(IListenerService listeners)
        {
            _listeners = listeners;
        }

        [HttpGet]
        public IActionResult GetListeners()
        {
            var listeners = _listeners.GetListeners();
            return Ok(listeners);
        }

        [HttpGet("{name}")]
        public IActionResult GetListener(string name)
        {
            var listener = _listeners.GetListener(name);
            if (listener is null) return NotFound();

            return Ok(listener);
        }

        [HttpPost]
        public IActionResult StartListener([FromBody] StartHttpListenerRequest request)
        {
            var listener = new HttpListener(request.Name, request.BindPort);
#pragma warning disable CS4014
            listener.Start();
#pragma warning restore CS4014
            
            _listeners.AddListener(listener);

            var root = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{HttpContext.Request.Path}";
            var path = $"{root}/{listener.Name}";

            return Created(path, listener);
        }

        [HttpDelete("{name}")]
        public IActionResult StopListener(string name)
        {
            var listener = _listeners.GetListener(name);
            if (listener is null) return NotFound();
            
            listener.Stop();
            _listeners.RemoveListener(listener);

            return NoContent();
        }
    }
}