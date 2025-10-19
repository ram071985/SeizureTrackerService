using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Service;

public interface ISeizureTrackerService
{
    public Task<SeizureFormDTO> AddActivityLog(SeizureFormDTO seizureFormDto);
}