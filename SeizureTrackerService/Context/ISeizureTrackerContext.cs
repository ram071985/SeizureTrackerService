using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public interface ISeizureTrackerContext
{
    public Task<List<SeizureActivityHeader>> GetActivityHeaders();
    public Task<bool> GetActivityHeadersFromToday();
    public Task AddSeizureActivityHeader(SeizureActivityHeader seizureActivityHeader);
    public Task AddSeizureActivityDetail(SeizureActivityDetail seizureActivityDetail);
}