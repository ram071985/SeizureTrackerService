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
            SeizureTime = entity.SeizureTime.ToString(),
            SeizureType = entity.SeizureType,
            Comments = entity.Comments
        };
    }
    
}