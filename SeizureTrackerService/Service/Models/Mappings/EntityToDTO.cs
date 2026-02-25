using System.Runtime.InteropServices;
using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class EntityToDTO
{
    internal static SeizureActivityHeaderDTO MapSeizureActivityHeaderEntityToDTO(this ManageLogHeaders entity)
    {
        string tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? "Central Standard Time" 
            : "America/Chicago";
        
        var cstZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);
        
        return new SeizureActivityHeaderDTO()
        {
            Id = entity.Id,
            Date = TimeZoneInfo.ConvertTime(entity.Date.DateTime, cstZone).ToString("D"),
            DailyTotal = entity.DailyTotal
        };
    }

    internal static SeizureActivityDetailDTO MapSeizureActivityDetailEntityToDTO(this GetActivityDetailByHeaderId entity)
    {
        string tzId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) 
            ? "Central Standard Time" 
            : "America/Chicago";
        
        var cstZone = TimeZoneInfo.FindSystemTimeZoneById(tzId);
        
        return new SeizureActivityDetailDTO()
        {
            SeizureId = entity.SeizureId,
            LogId = entity.LogId,
            SeizureTime = TimeZoneInfo.ConvertTime(entity.SeizureTime.DateTime, cstZone).ToString("h:mm tt"),
            SeizureType = entity.SeizureType,
            Comments = entity.Comments
        };
    }
    
}