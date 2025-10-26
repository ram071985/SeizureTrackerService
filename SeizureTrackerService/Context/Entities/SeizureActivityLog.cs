using System.Runtime.InteropServices.JavaScript;

namespace SeizureTrackerService.Context.Entities;

public partial class SeizureActivityLog
{
    public int Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? SeizureDate { get; set; }
    public string? SeizureDescription { get; set; }
    public string? SeizureType { get; set; }
    public decimal? SleepAmount { get; set; }
    public int? SeizureIntensity { get; set; }
    public string? Notes { get; set; }
    public bool? MedicationChange { get; set; }
    public string? MedicationChangeDescription{ get; set; }
}