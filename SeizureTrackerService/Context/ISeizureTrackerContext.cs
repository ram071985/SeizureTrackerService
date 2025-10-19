using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Context;

public interface ISeizureTrackerContext
{
    public Task AddSeizureActivityLog(Seizure activityLog);
}