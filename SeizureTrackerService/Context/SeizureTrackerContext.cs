using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Constants;
using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Context;

public class SeizureTrackerContext(DbContextOptions<SeizureTrackerContext> options, IConfiguration config)
    : DbContext(options), ISeizureTrackerContext
{
    private readonly string _schema = config.GetValue<string>(AppSettings.SeizureTrackerSchema);


    // public DbSet<SeizureActivityLog> Seizures { get; set; }
    public DbSet<SeizureActivityHeader> SeizureActivityHeader { get; set; }
    public DbSet<SeizureActivityDetail> SeizureActivityDetail { get; set; }
    public DbSet<ManageLogHeaders> ManageLogHeaders { get; set; }
    public DbSet<GetActivityDetailByHeaderId> GetActivityDetailByHeadersId { get; set; }
    public DbSet<WhiteList> WhiteList { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_schema);
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
            return await ManageLogHeaders.OrderByDescending(d => d.Date).ToListAsync();
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
            var getActivityDetailsByHeaderId = (_schema == Schema.Dev)
                 ? StoredProcedures.GetActivityLogDetailsByHeaderIdDev 
                 :  StoredProcedures.GetActivityLogDetailsByHeaderIdProd;
            return await GetActivityDetailByHeadersId
                .FromSqlInterpolated($"{getActivityDetailsByHeaderId}{headerId}")
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

            var checkWhitelistSproc = (_schema == Schema.Dev)
                ? StoredProcedures.CheckWhiteListSprocDev
                : StoredProcedures.CheckWhiteListSproc;
            await Database.ExecuteSqlRawAsync(checkWhitelistSproc, emailParam, outputParam);

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

    # region Patch

    public async Task PatchSeizureActivityDetail(SeizureActivityDetail activityLog)
    {
        try
        {
            var existingSeizureDetail = await SeizureActivityDetail
                .FirstOrDefaultAsync(log => log.SeizureId == activityLog.SeizureId);

            if (existingSeizureDetail == null)
                throw new DbUpdateException("Activity log doesn't exist.. Failed to update database record.");

            if (!string.IsNullOrEmpty(activityLog.SeizureType))
                existingSeizureDetail.SeizureType = activityLog.SeizureType;
            if (!string.IsNullOrEmpty(activityLog.Comments))
                existingSeizureDetail.Comments = activityLog.Comments;

            await SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new NonExistantRecordException(ex.Message, ex);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    #endregion
}