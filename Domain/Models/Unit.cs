using Domain.Enums;

namespace Domain.Models;

public class Unit
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public EntityStatus Status { get; set; }
    
    public virtual List<Service> Services { get; set; }
}