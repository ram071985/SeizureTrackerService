namespace SeizureTrackerService.Context.Entities;

public class GetActivityDetailByHeaderId
{
    public int SeizureId { get; set; }
    public DateTime SeizureTime { get; set; }
    public string SeizureType { get; set; }
    public string? Comments { get; set; }
}