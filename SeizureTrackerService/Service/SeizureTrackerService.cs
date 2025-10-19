using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Service;

public class SeizureTrackerService(IConfiguration config) : ISeizureTrackerService
{
    private readonly IConfiguration _config = config;
    
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
            return new();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);

            throw;
        }
    }

    #region Private methods

    private async Task<SeizureFormDTO> AddActivityLog() => await ;

    #endregion

}