using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public interface ISeizureTrackerContext
{
    Task<List<ManageLogHeaders>> GetActivityHeaders();
    Task<SeizureActivityHeader?> GetActivityHeadersFromToday();
    Task<List<GetActivityDetailByHeaderId>> GetActivityDetailsByHeaderId(int headerId);
    Task<int> AddSeizureActivityHeader(SeizureActivityHeader seizureActivityHeader);
    Task AddSeizureActivityDetail(SeizureActivityDetail seizureActivityDetail);
    Task<bool> CheckWhiteListSproc(string email);
}