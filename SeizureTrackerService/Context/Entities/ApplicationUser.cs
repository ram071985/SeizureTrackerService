using Microsoft.AspNetCore.Identity;

namespace SeizureTrackerService.Context.Entities;

public class ApplicationUser : IdentityUser
{
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public DateTime? LastSeizureOccurrence { get; set; }
}