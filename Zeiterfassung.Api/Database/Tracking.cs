using System.Text.Json.Serialization;

namespace Zeiterfassung.Api.Database;

public class Tracking
{
    /// <summary>
    /// Primary key
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// The type of the tracking
    /// </summary>
    public TrackingType TrackingType { get; set; }

    /// <summary>
    /// The timestamp in UTC, when the record was created
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Each tracking needs to have a user. (required)
    /// </summary>
    /// <remarks>Foreign key to <see cref="User.Trackings"/></remarks>
    [JsonIgnore]
    public User User { get; set; } = null!;
}
