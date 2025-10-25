using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class DtoToEntity
{
    internal static SeizureActivityLog MapSeizureActivityLogDtoToEntity(this SeizureFormDto source)
    {
        var isSeizureTimeValid = DateTime.TryParse(source.SeizureTime, out var seizureTime);
        
        return new SeizureActivityLog
        {
            SeizureDescription = source.SeizureDescription,
            CreatedDate = DateTime.Now,
            SeizureDate = isSeizureTimeValid ? seizureTime : null,
            SeizureType = source.SeizureType
        };
    }
}