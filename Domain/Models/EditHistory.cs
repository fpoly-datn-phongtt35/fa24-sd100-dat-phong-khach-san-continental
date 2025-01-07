using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Models;

public class EditHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Guid RoomBookingDetailId { get; set; }
    public For For { get; set; }
    public string? Content { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public virtual RoomBookingDetail? RoomBookingDetail { get; set; }
}