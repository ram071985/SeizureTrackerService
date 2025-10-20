using System.Runtime.InteropServices.JavaScript;

namespace SeizureTrackerService.Context.Entities;

public partial class SeizureActivityLog
{
    public int Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? SeizureTime { get; set; }
    public string? SeizureDescription { get; set; }
    // public string? AmPm { get; set; }
    // public int? SeizureStrength { get; set; }
    // public decimal? KetonesLevel { get; set; }
    // public string? SeizureType { get; set; }
    // public int? SleepAmount { get; set; }
    // public string? Notes { get; set; }
    // public bool? MedicationChange { get; set; }
    // public string? MedicationChangeExplanation { get; set; }
}