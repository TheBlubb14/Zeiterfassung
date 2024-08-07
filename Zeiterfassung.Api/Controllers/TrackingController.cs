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

    [HttpGet("IsRunning")]
    public bool IsRunning(AppDbContext db, CallerContext callerContext)
    {
        return db.Users
            .Include(x => x.Trackings)
            .Any(x => x.Id == callerContext.User.Id &&
                x.Trackings != null &&
                x.Trackings.Count > 0 &&
                x.Trackings.OrderBy(x => x.Id).Last().TrackingType == TrackingType.Started);
    }

    [HttpGet("GetAll")]
    public List<Tracking> GetAll(AppDbContext db, CallerContext callerContext)
    {
        return db.Users
            .Include(x => x.Trackings)
            .First(x => x.Id == callerContext.User.Id)
            .Trackings;
    }

    [HttpGet("GetAll/{from}/{to}")]
    public List<Tracking> GetAll(AppDbContext db, CallerContext callerContext, DateOnly? from, DateOnly? to)
    {
        return db.Users
            .Include(x => x.Trackings)
            .FirstOrDefault(x => x.Id == callerContext.User.Id)?
            .Trackings
            .Where(x => (from is null || DateOnly.FromDateTime(x.TimeStamp) >= from) &&
                        (to is null || DateOnly.FromDateTime(x.TimeStamp) <= to))
            .ToList() ?? [];
    }
}
