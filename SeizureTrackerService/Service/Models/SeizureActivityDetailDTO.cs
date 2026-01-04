namespace SeizureTrackerService.Service.Models;

public class SeizureActivityDetailDTO
{
    public int SeizureId { get; set; }
    public int LogId { get; set; }
    public string SeizureTime { get; set; }
    public string SeizureType { get; set; }
    public string? Comments { get; set; }
}