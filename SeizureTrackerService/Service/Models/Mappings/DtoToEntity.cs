using System.Runtime.InteropServices;
using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class DtoToEntity
{
    internal static SeizureActivityHeader MapSeizureActivityHeaderDTOToEntity(this SeizureActivityDetailDTO detail)
    {
        
        var parsedDate = DateTime.Parse(detail.SeizureDate);
        
        string tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? "Central Standard Time" 
            : "America/Chicago";
        
        var cstZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);
        
        var localTime = DateTime.SpecifyKind(parsedDate, DateTimeKind.Unspecified);

        TimeSpan currentOffset = cstZone.GetUtcOffset(localTime);
        
        return new SeizureActivityHeader
        {
            Date = new DateTimeOffset(localTime, currentOffset)
        };
    }
    internal static SeizureActivityDetail MapSeizureActivityDetailDTOToEntity(this SeizureActivityDetailDTO detail)
    {
        var parsedDate = DateTime.Parse(detail.SeizureTime);
        
        string tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? "Central Standard Time" 
            : "America/Chicago";
        
        var cstZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);
        
        var localTime = DateTime.SpecifyKind(parsedDate, DateTimeKind.Unspecified);

        TimeSpan currentOffset = cstZone.GetUtcOffset(localTime);

        return new SeizureActivityDetail
        {
            SeizureId = detail.SeizureId,
            SeizureType = detail.SeizureType,
            LogId = detail.LogId,
            Comments = detail.Comments,
            SeizureTime = new DateTimeOffset(localTime, currentOffset)
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