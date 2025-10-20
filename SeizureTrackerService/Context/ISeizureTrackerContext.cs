using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public interface ISeizureTrackerContext
{
    public Task AddSeizureActivityLog(SeizureActivityLog activityLog);
}