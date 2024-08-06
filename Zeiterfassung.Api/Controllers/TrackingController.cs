using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zeiterfassung.Api.Database;

namespace Zeiterfassung.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TrackingController(ILogger<TrackingController> logger) : ControllerBase
{
    [HttpPost("Start")]
    public DateTime Start(AppDbContext db, CallerContext callerContext)
    {
        var startTime = DateTime.UtcNow;

        var user = db.Users
            .Include(x => x.Trackings)
            .Single(x => x.Id == callerContext.User.Id);

        user.Trackings.Add(new Tracking()
        {
            TimeStamp = startTime,
            TrackingType = TrackingType.Started,
        });

        db.SaveChanges();

        return startTime;
    }

    [HttpPost("Stop")]
    public DateTime Stop(AppDbContext db, CallerContext callerContext)
    {
        var stopTime = DateTime.UtcNow;

        var user = db.Users
            .Include(x => x.Trackings)
            .Single(x => x.Id == callerContext.User.Id);

        user.Trackings.Add(new Tracking()
        {
            TimeStamp = stopTime,
            TrackingType = TrackingType.Stopped,
        });

        db.SaveChanges();

        return stopTime;
    }

    [HttpPost("IsRunning")]
    public bool IsRunning(AppDbContext db, CallerContext callerContext)
    {
        return db.Users
            .Include(x => x.Trackings)
            .Any(x => x.Id == callerContext.User.Id && 
                x.Trackings != null && 
                x.Trackings.Count > 0 &&
                x.Trackings.OrderBy(x => x.Id).Last().TrackingType == TrackingType.Started);
    }
}
