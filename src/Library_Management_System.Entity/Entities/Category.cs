using Library_Management_System.Core.Entity;

namespace Library_Management_System.Entity.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = null!;
    public string BackStory { get; set; } = null!;
    
    public string? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    
    public ICollection<Book> Books { get; set; } = null!;
}