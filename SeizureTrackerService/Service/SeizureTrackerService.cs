using SeizureTrackerService.Context;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Service;

public class SeizureTrackerService(IConfiguration config, ISeizureTrackerService seizureTrackerService, ISeizureTrackerContext seizureTrackerContext) : ISeizureTrackerService
{
    private readonly IConfiguration _config = config;
    private readonly ILogger<SeizureTrackerService> _logger;
    private readonly ISeizureTrackerContext _seizureTrackerContext = seizureTrackerContext;
    private readonly ISeizureTrackerService _seizureTrackerService = seizureTrackerService;
    
    public async Task<SeizureFormDTO> AddActivityLog(SeizureFormDTO form)
    {
        // DateTime.TryParse(form.CreatedDate, out DateTime createdDate);
        //
        // DateTime? timeOfSeizure = null;

        try
        {
            // if (form.TimeOfSeizure is not null)
            //     timeOfSeizure = createdDate + createTimeOfSeizureTimeStamp(form);
            //
            // var log = form.MapSeiureLogDTOToEntityModel(createdDate, timeOfSeizure);
            //
            // await addSeizureLog(log);
            //
            // return log.MapSeizureLogEntityToDTO();
            return await AddActivityLog();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    #region Private methods

    private async Task AddActivityLog(Seizure activityLog) => await _seizureTrackerContext.AddSeizureActivityLog(activityLog);

    #endregion

}