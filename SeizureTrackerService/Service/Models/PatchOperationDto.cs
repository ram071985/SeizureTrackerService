namespace SeizureTrackerService.Service.Models;

public class PatchOperationDto
{
    public string op { get; set; } = default!;   
    public string path { get; set; } = default!; 
    public object? value { get; set; }          
}
