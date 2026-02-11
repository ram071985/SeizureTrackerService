using SeizureTrackerService.Context;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Service.Models;
using SeizureTrackerService.Service.Models.Mappings;
using SeizureTrackerService.Service.Models.Mappings;

namespace SeizureTrackerService.Service;

public class SeizureTrackerService(IConfiguration config, ISeizureTrackerContext seizureTrackerContext)
    : ISeizureTrackerService
{
    private readonly IConfiguration _config = config;
    private readonly ILogger<SeizureTrackerService> _logger;
    private readonly ISeizureTrackerContext _seizureTrackerContext = seizureTrackerContext;

    public async Task<List<SeizureActivityHeaderDTO>> GetSeizureActivityHeaders()
    {
        try
        {
            var activityHeaders = await GetActivityHeaders();

            return activityHeaders.Select(x => x.MapSeizureActivityHeaderEntityToDTO()).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            throw;
        }
    }

    public async Task<List<SeizureActivityDetailDTO>> GetSeizureActivityDetailsByHeaderId(int headerId)
    {
        try
        {
            var activityHeaders = await GetActivityDetailsByHeaderId(headerId);

            return activityHeaders.Select(x => x.MapSeizureActivityDetailEntityToDTO()).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            throw;
        }
    }

    public async Task<bool> CheckWhiteListSproc(string email)
    {
        try
        {
            return await CheckWhiteListSprocContext(email);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    public async Task AddActivityLog(SeizureActivityDetailDTO log)
    {
        try
        {
            var seizureActivityToday = await GetActivityHeadersFromToday();
            var mappedActivityDetail = log.MapSeizureActivityDetailDTOToEntity();

            if (seizureActivityToday == null)
            {
                var mappedActivityHeader = log.MapSeizureActivityHeaderDTOToEntity();

                var headerId = await AddActivityHeaderLog(mappedActivityHeader);

                mappedActivityDetail.LogId = headerId;
            }
            else
            {
                mappedActivityDetail.LogId = seizureActivityToday.Id;
            }

            await AddActivityDetailLog(mappedActivityDetail);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    public async Task PatchActivityLog(SeizureActivityDetailDTO seizureActivityDetail)
    {
        try
        {
            await PatchActivityDetailLog(seizureActivityDetail.MapSeizureActivityDetailDTOToEntity());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    #region Private methods

    #region Get

    private async Task<SeizureActivityHeader?> GetActivityHeadersFromToday() =>
        await _seizureTrackerContext.GetActivityHeadersFromToday();

    private async Task<List<ManageLogHeaders>> GetActivityHeaders() =>
        await _seizureTrackerContext.GetActivityHeaders();

    private async Task<List<GetActivityDetailByHeaderId>> GetActivityDetailsByHeaderId(int headerId) =>
        await _seizureTrackerContext.GetActivityDetailsByHeaderId(headerId);

    private async Task<bool> CheckWhiteListSprocContext(string email) =>
        await _seizureTrackerContext.CheckWhiteListSproc(email);

    #endregion


    #region Add

    private async Task<int> AddActivityHeaderLog(SeizureActivityHeader activityHeader) =>
        await _seizureTrackerContext.AddSeizureActivityHeader(activityHeader);

    private async Task AddActivityDetailLog(SeizureActivityDetail activityDetail) =>
        await _seizureTrackerContext.AddSeizureActivityDetail(activityDetail);

    private async Task PatchActivityDetailLog(SeizureActivityDetail seizureActivityDetail) =>
        await _seizureTrackerContext.PatchSeizureActivityDetail(seizureActivityDetail);

    #endregion

    #endregion
}