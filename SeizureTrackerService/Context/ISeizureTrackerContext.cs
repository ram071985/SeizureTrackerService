using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public interface ISeizureTrackerContext
{
    public Task<List<SeizureActivityHeader>> GetActivityHeaders();
    public Task<SeizureActivityHeader?> GetActivityHeadersFromToday();
    public Task<int> AddSeizureActivityHeader(SeizureActivityHeader seizureActivityHeader);
    public Task AddSeizureActivityDetail(SeizureActivityDetail seizureActivityDetail);
}