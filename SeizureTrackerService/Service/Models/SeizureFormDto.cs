namespace SeizureTrackerService.Service.Models;

public class  SeizureFormDto
{
    public int? Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? SeizureDate { get; set; }
    public string? SeizureDescription { get; set; }
    public string? SeizureType { get; set; }
    public string? SleepAmount { get; set; }
    public string? SeizureIntensity { get; set; }
    public string? Notes { get; set; }
    public string? MedicationChange { get; set; }
    public string? MedicationChangeDescription { get; set; }
}