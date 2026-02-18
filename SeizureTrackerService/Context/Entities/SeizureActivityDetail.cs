

using System.ComponentModel.DataAnnotations;

namespace SeizureTrackerService.Context.Entities;

public partial class SeizureActivityDetail
{
    [Key]
    public int SeizureId { get; set; }
    public int LogId { get; set; }
    public DateTimeOffset SeizureTime { get; set; }
    public string SeizureType { get; set; }
    public string? Comments { get; set; }
}