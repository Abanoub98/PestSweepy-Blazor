using System.Text.Json.Serialization;

namespace Dashboard.Blazor.Models.Consts;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum VisitStatus
{
    Scheduled = 1,
    Completed,
    ReScheduled,
    Missed,
    Cancelled
}
