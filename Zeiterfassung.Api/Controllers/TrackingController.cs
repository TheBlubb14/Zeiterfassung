using Microsoft.AspNetCore.Mvc;

namespace Zeiterfassung.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackingController(ILogger<TrackingController> logger) : ControllerBase
{
    [HttpPost("Start")]
    public DateTime Start()
    {
        var startTime = DateTime.UtcNow;

        return startTime;
    }

    [HttpPost("Stop")]
    public DateTime Stop()
    {
        var stopTime = DateTime.UtcNow;

        return stopTime;
    }

    [HttpPost("IsRunning")]
    public bool IsRunning()
    {
        return false;
    }
}
