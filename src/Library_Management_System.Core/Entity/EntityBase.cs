namespace Library_Management_System.Core.Entity;

public class EntityBase : IEntityBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CreatedId { get; set; } = null!;
    public string? UpdatedId { get; set; }
    public string? DeletedId { get; set; }

    public DateTime CreateDateTime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdateDateTime { get; set; }
    public DateTime? DeleteDateTime { get; set; }

    public bool IsDeleted { get; set; } = false;
}