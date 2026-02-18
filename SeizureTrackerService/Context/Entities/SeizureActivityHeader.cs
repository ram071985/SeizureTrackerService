using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SeizureTrackerService.Context.Entities;

public partial class SeizureActivityHeader
{
    [Key]
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
}