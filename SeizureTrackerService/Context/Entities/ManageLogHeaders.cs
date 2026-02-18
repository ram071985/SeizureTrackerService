using Microsoft.EntityFrameworkCore;

namespace SeizureTrackerService.Context.Entities;

// [Keyless]
public partial class ManageLogHeaders
{
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public int DailyTotal { get; set; }
}