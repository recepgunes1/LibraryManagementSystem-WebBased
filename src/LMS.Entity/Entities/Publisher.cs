using LMS.Core.Entity;

namespace LMS.Entity.Entities;

public class Publisher : EntityBase
{
    public string Name { get; set; } = null!;
    public string BackStory { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = null!;
}