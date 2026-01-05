using Microsoft.EntityFrameworkCore;

namespace SeizureTrackerService.Context.Entities;

// [Keyless]
public partial class ManageLogHeaders
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int DailyTotal { get; set; }
}