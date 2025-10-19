namespace SeizureTrackerService.Context.Entities;

public partial class Seizure
{
    public int ID { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? TimeOfSeizure { get; set; }
    public string? AmPm { get; set; }
    public int? SeizureStrength { get; set; }
    public decimal? KetonesLevel { get; set; }
    public string? SeizureType { get; set; }
    public int? SleepAmount { get; set; }
    public string? Notes { get; set; }
    public bool? MedicationChange { get; set; }
    public string? MedicationChangeExplanation { get; set; }
}