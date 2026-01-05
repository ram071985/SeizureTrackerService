using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Service;

public interface ISeizureTrackerService
{
    public Task<List<SeizureActivityHeaderDTO>> GetSeizureActivityHeaders();
    public Task<List<SeizureActivityDetailDTO>> GetSeizureActivityDetailsByHeaderId(int headerId);
    public Task AddActivityLog(SeizureActivityDetailDTO seizureFormDto);
}