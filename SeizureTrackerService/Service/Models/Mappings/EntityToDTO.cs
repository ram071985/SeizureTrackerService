using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class EntityToDTO
{
    internal static SeizureActivityHeaderDTO MapSeizureActivityHeaderEntityToDTO(this ManageLogHeaders entity)
    {
        return new SeizureActivityHeaderDTO()
        {
            Id = entity.Id,
            Date = entity.Date.ToShortDateString(),
            DailyTotal = entity.DailyTotal
        };
    }

    internal static SeizureActivityDetailDTO MapSeizureActivityDetailEntityToDTO(this GetActivityDetailByHeaderId entity)
    {
        return new SeizureActivityDetailDTO()
        {
            SeizureId = entity.SeizureId,
            LogId = entity.LogId,
            SeizureDate = entity.SeizureTime.ToShortDateString(),
            SeizureTime = entity.SeizureTime.ToShortTimeString(),
            SeizureType = entity.SeizureType,
            Comments = entity.Comments
        };
    }
    
}