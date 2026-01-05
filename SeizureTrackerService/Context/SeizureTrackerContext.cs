using Microsoft.EntityFrameworkCore;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Constants;

namespace SeizureTrackerService.Context;

public class SeizureTrackerContext(DbContextOptions<SeizureTrackerContext> options, IConfiguration config)
    : DbContext(options), ISeizureTrackerContext
{
    // public DbSet<SeizureActivityLog> Seizures { get; set; }
    public DbSet<SeizureActivityHeader> SeizureActivityHeader { get; set; }
    public DbSet<SeizureActivityDetail> SeizureActivityDetail { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(config.GetValue<string>(AppSettings.SeizureTrackerSchema));
        modelBuilder.Entity<SeizureActivityHeader>().ToTable(Tables.SeizureActivityHeader);
        modelBuilder.Entity<SeizureActivityDetail>().ToTable(Tables.SeizureActivityDetail);
    }
#region Get
public async Task<List<SeizureActivityHeader>> GetActivityHeaders()
{
    try
    {
        return await SeizureActivityHeader.ToListAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        
        throw;
    }
}
public async Task<SeizureActivityHeader?> GetActivityHeadersFromToday()
{
    try
    {
        return await SeizureActivityHeader.FirstOrDefaultAsync(x => x.Date.Date == DateTime.Now.Date);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        
        throw;
    }
}
#endregion
    #region Add
    public async Task<int> AddSeizureActivityHeader(SeizureActivityHeader activityLog)
    {
        try
        {
            await SeizureActivityHeader.AddAsync(activityLog);

            await SaveChangesAsync();
            
            return activityLog.Id;
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
            await SeizureActivityDetail.AddAsync(activityLog);

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