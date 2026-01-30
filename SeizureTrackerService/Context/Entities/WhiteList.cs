using System.ComponentModel.DataAnnotations;

namespace SeizureTrackerService.Context.Entities;

public class WhiteList
{
    [Key] 
    public string Email { get; set; } // The primary key
    public bool IsActive { get; set; } = true;
}