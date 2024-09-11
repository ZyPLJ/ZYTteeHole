using Microsoft.AspNetCore.Mvc;
using ZYTreeHole_Services.Services;

namespace ZYTreeHole.Controllers.API;

[Route("[controller]")]
public class ConnectionController : ControllerBase
{
    private readonly ChatHub _chatHub;

    public ConnectionController(ChatHub chatHub)
    {
        _chatHub = chatHub;
    }

    [HttpGet("[action]")]
    public IActionResult GetConnectionCount()
    {
        int connectionCount = _chatHub.GetConnectionCount();
        return Ok(new { ConnectionCount = connectionCount });
    }
}