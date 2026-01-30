using Microsoft.Data.SqlClient;
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
    public DbSet<ManageLogHeaders> ManageLogHeaders { get; set; }
    public DbSet<GetActivityDetailByHeaderId> GetActivityDetailByHeadersId { get; set; }
    public DbSet<WhiteList> WhiteList { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(config.GetValue<string>(AppSettings.SeizureTrackerSchema));
        modelBuilder.Entity<SeizureActivityHeader>().ToTable(Tables.SeizureActivityHeader);
        modelBuilder.Entity<SeizureActivityDetail>().ToTable(Tables.SeizureActivityDetail);
        modelBuilder.Entity<ManageLogHeaders>().ToView(Views.GetManageLogsView);
        modelBuilder.Entity<WhiteList>().ToTable(Tables.WhiteList);
    }

    #region Get

    public async Task<List<ManageLogHeaders>> GetActivityHeaders()
    {
        try
        {
            return await ManageLogHeaders.ToListAsync();
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

    public async Task<List<GetActivityDetailByHeaderId>> GetActivityDetailsByHeaderId(int headerId)
    {
        try
        {
            return await GetActivityDetailByHeadersId
                .FromSqlInterpolated($"EXEC dev.GetActivityLogDetailsByHeaderId @HeaderId={headerId}")
                .ToListAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            throw;
        }
    }

    public async Task<bool> CheckWhiteListSproc(string email)
    {
        var outputParam = new SqlParameter
        {
            ParameterName = "@IsAuthorized",
            SqlDbType = System.Data.SqlDbType.Bit,
            Direction = System.Data.ParameterDirection.Output // Essential for reading data back
        };

        try
        {
            var emailParam = new SqlParameter("@Email", email);

            await Database.ExecuteSqlRawAsync(StoredProcedures.CheckWhiteListSproc, emailParam, outputParam);

            bool isAuthorized = (bool)outputParam.Value;

            return isAuthorized;
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