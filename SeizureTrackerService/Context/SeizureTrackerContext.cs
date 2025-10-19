using Microsoft.EntityFrameworkCore;
using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public class SeizureTrackerContext(DbContextOptions<SeizureTrackerContext> options) : DbContext(options)
{
    public DbSet<Seizure> Seizures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("SeizureTracker");
        modelBuilder.Entity<Seizure>().ToTable("seizureActivityLog");
    }
    
    public async Task AddSeizureActivityLog(Seizure activityLog)
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