using LMS.Core.Entity;

namespace LMS.Entity.Entities;

public class Author : EntityBase
{
    public string FullName { get; set; } = null!;
    public string? BackStory { get; set; }

    public ICollection<Book> Books { get; set; } = null!;
}