using SeizureTrackerService.Context;
using SeizureTrackerService.Context.Entities;
using SeizureTrackerService.Service.Models;
using SeizureTrackerService.Service.Models.Mappings;

namespace SeizureTrackerService.Service;

public class SeizureTrackerService(IConfiguration config, ISeizureTrackerContext seizureTrackerContext) : ISeizureTrackerService
{
    private readonly IConfiguration _config = config;
    private readonly ILogger<SeizureTrackerService> _logger;
    private readonly ISeizureTrackerContext _seizureTrackerContext = seizureTrackerContext;
    
    public async Task AddActivityLog(SeizureFormDto activityForm)
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
            var mappedActivityLog = activityForm.MapSeizureActivityLogDtoToEntity();
            
            await AddActivityLog(mappedActivityLog);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    #region Private methods

    private async Task AddActivityLog(SeizureActivityLog activityLog) => await _seizureTrackerContext.AddSeizureActivityLog(activityLog);

    #endregion

}