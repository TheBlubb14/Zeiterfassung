namespace Zeiterfassung.Api.Database;

public class User
{
    /// <summary>
    /// Primary key. Unique user id. Used to authenticate
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The date the user was created
    /// </summary>
    /// <remarks>Will be set by database</remarks>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Each user has multiple trackings. (required)
    /// </summary>
    /// <remarks>Collection navigation of <see cref="Tracking"/></remarks>
    public List<Tracking> Trackings { get; set; } = [];
}
