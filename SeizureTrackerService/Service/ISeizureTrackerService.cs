using SeizureTrackerService.Service.Models;

namespace SeizureTrackerService.Service;

public interface ISeizureTrackerService
{
    Task<List<SeizureActivityHeaderDTO>> GetSeizureActivityHeaders();
    Task<List<SeizureActivityDetailDTO>> GetSeizureActivityDetailsByHeaderId(int headerId);
    Task AddActivityLog(SeizureActivityDetailDTO seizureFormDto);
    Task<bool> CheckWhiteListSproc(string email);
}