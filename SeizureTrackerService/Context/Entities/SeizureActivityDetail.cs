namespace SeizureTrackerService.Context.Entities;

public partial class SeizureActivityDetail
{
    public int SeizureId { get; set; }
    public int LogId { get; set; }
    public DateTime SeizureTime { get; set; }
    public string SeizureType { get; set; }
    public string? Comments { get; set; }
}