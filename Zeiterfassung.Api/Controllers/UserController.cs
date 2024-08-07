using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Zeiterfassung.Api.Database;

namespace Zeiterfassung.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(ILogger<UserController> logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("Register")]
    public Ok<string> Register(AppDbContext db)
    {
        var user = db.Users.Add(new Database.User());
        db.SaveChanges();
        return TypedResults.Ok(user.Entity.Id.ToString());
    }

    [AllowAnonymous]
    [HttpGet("Ping")]
    public Ok<string> Ping()
    {
        return TypedResults.Ok("pong");
    }
}
