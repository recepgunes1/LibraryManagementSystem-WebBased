using Library_Management_System.Core.Entity;

namespace Library_Management_System.Entity.Entities;

public class Author : EntityBase
{
    public string FullName { get; set; } = null!;
    public string? BackStory { get; set; }

    public ICollection<Book> Books { get; set; } = null!;
}