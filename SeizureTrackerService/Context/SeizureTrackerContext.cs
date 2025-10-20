using Microsoft.EntityFrameworkCore;
using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public class SeizureTrackerContext(DbContextOptions<SeizureTrackerContext> options) : DbContext(options), ISeizureTrackerContext
{
    public DbSet<SeizureActivityLog> Seizures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("dev");
        modelBuilder.Entity<SeizureActivityLog>().ToTable("SeizureActivityLog");
    }
    
    public async Task AddSeizureActivityLog(SeizureActivityLog activityLog)
    {
        try
        {
            await Seizures.AddAsync(activityLog);

            await SaveChangesAsync();
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }
}