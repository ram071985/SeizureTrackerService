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

    #region Private methods

    #region Get

    private async Task<SeizureActivityHeader?> GetActivityHeadersFromToday() =>
        await _seizureTrackerContext.GetActivityHeadersFromToday();

    private async Task<List<SeizureActivityHeader>> GetActivityHeaders() =>
        await _seizureTrackerContext.GetActivityHeaders();

    #endregion


    #region Add

    private async Task<int> AddActivityHeaderLog(SeizureActivityHeader activityHeader) =>
        await _seizureTrackerContext.AddSeizureActivityHeader(activityHeader);

    private async Task AddActivityDetailLog(SeizureActivityDetail activityDetail) =>
        await _seizureTrackerContext.AddSeizureActivityDetail(activityDetail);

    #endregion

    #endregion
}