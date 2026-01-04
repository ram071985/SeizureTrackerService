using SeizureTrackerService.Context.Entities;

namespace SeizureTrackerService.Service.Models.Mappings;

internal static class EntityToDTO
{
    internal static SeizureActivityHeaderDTO MapSeizureActivityHeaderEntityToDTO(this SeizureActivityHeader entity)
    {
        return new SeizureActivityHeaderDTO()
        {
            Id = entity.Id,
            Date = entity.Date.ToString()
        };
    }

    internal static SeizureActivityDetailDTO MapSeizureActivityDetailEntityToDTO(this SeizureActivityDetail entity)
    {
        return new SeizureActivityDetailDTO()
        {
            SeizureId = entity.SeizureId,
            LogId = entity.LogId,
            SeizureTime = entity.SeizureTime.ToString(),
            SeizureType = entity.SeizureType,
            Comments = entity.Comments
        };
    }
}