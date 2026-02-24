using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class EntityToDTO
{
    internal static SeizureActivityHeaderDTO MapSeizureActivityHeaderEntityToDTO(this ManageLogHeaders entity)
    {
        var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        
        return new SeizureActivityHeaderDTO()
        {
            Id = entity.Id,
            Date = TimeZoneInfo.ConvertTime(entity.Date.DateTime, cstZone).ToString("D"),
            DailyTotal = entity.DailyTotal
        };
    }

    internal static SeizureActivityDetailDTO MapSeizureActivityDetailEntityToDTO(this GetActivityDetailByHeaderId entity)
    {
        var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

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