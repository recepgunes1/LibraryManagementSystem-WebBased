using Library_Management_System.Core.Entity;

namespace Library_Management_System.Entity.Entities;

public class Publisher : EntityBase
{
    public string Name { get; set; } = null!;
    public string BackStory { get; set; } = null!;

    public ICollection<Book> Books { get; set; } = null!;
}