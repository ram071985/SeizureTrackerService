using Microsoft.EntityFrameworkCore;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Constants;

namespace SeizureTrackerService.Context;

public class SeizureTrackerContext(DbContextOptions<SeizureTrackerContext> options, IConfiguration config)
    : DbContext(options), ISeizureTrackerContext
{
    public DbSet<SeizureActivityLog> Seizures { get; set; }
    public DbSet<SeizureActivityHeader> SeizureActivityHeaders { get; set; }
    public DbSet<SeizureActivityDetail> SeizureActivityDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(config.GetValue<string>(AppSettings.SeizureTrackerSchema));
        modelBuilder.Entity<SeizureActivityLog>().ToTable(Tables.SeizureActivityHeader);
        modelBuilder.Entity<SeizureActivityLog>().ToTable(Tables.SeizureActivityDetail);
    }
#region Get
public async Task<List<SeizureActivityHeader>> GetActivityHeaders()
{
    try
    {
        return await SeizureActivityHeaders.ToListAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        
        throw;
    }
}
public async Task<bool> GetActivityHeadersFromToday()
{
    try
    {
        return await SeizureActivityHeaders.AnyAsync(x => x.Date.Date == DateTime.Now.Date);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        
        throw;
    }
}
#endregion
    #region Add
    public async Task AddSeizureActivityHeader(SeizureActivityHeader activityLog)
    {
        try
        {
            await SeizureActivityHeaders.AddAsync(activityLog);

            await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }
    
    public async Task AddSeizureActivityDetail(SeizureActivityDetail activityLog)
    {
        try
        {
            await SeizureActivityDetails.AddAsync(activityLog);

            await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }
    #endregion
}