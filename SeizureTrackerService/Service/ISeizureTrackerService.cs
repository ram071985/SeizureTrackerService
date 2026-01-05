using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Service;

public interface ISeizureTrackerService
{
    public Task<List<SeizureActivityHeaderDTO>> GetSeizureActivityHeaders();
    public Task AddActivityLog(SeizureActivityDetailDTO seizureFormDto);
}