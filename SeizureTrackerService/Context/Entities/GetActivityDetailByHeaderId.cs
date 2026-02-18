using Microsoft.EntityFrameworkCore;

namespace SeizureTrackerService.Context.Entities;

[Keyless]
public class GetActivityDetailByHeaderId
{
    public int SeizureId { get; set; }
    public int LogId { get; set; }
    public DateTimeOffset SeizureTime { get; set; }
    public string SeizureType { get; set; }
    public string? Comments { get; set; }
}