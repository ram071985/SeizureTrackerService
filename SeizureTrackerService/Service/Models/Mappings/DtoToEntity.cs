using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class DtoToEntity
{
    internal static SeizureActivityHeader MapSeizureActivityHeaderDTOToEntity(this SeizureActivityDetailDTO detail)
    {
        return new SeizureActivityHeader
        {
            Date = DateTime.Parse(detail.SeizureTime),
        };
    }
    internal static SeizureActivityDetail MapSeizureActivityDetailDTOToEntity(this SeizureActivityDetailDTO detail)
    {
        return new SeizureActivityDetail
        {
            SeizureId = detail.SeizureId,
            LogId = detail.LogId,
            SeizureTime = DateTime.Parse(detail.SeizureTime),
        };
    }
    internal static SeizureActivityLog MapSeizureActivityLogDtoToEntity(this SeizureFormDto source)
    {
        var isSeizureTimeValid = DateTime.TryParse(source.SeizureDate, out var seizureTime);
        var isSleepAmountValid = decimal.TryParse(source.SleepAmount, out var sleepAmount);
        var isSeizureIntensityValid = int.TryParse(source.SeizureIntensity, out var seizureIntensity);
        var isDurationValid = decimal.TryParse(source.Duration, out var duration);
        
        return new SeizureActivityLog
        {
            SeizureDescription = source?.SeizureDescription,
            CreatedDate = DateTime.Now,
            SeizureDate = isSeizureTimeValid ? seizureTime : null,
            SeizureType = source?.SeizureType,
            SleepAmount = isSleepAmountValid ? sleepAmount : null,
            SeizureIntensity = isSeizureIntensityValid ? seizureIntensity : null,
            MedicationChange = source?.MedicationChange switch
            {
                "Yes" => true,
                "No" => false,
                _ => null
            },
            MedicationChangeDescription = source?.MedicationChangeDescription,
            Notes = source?.Notes,
            Duration = isDurationValid ? duration : null,
            ShortLog = source?.ShortLog
        };
    }
}