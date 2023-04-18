using Library_Management_System.Core.Entity;

namespace Library_Management_System.Entity.Entities;

public class Book : EntityBase
{
    public string Name { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public int Pages { get; set; }
    public int Amount { get; set; }
    public DateTime PublishedDateTime { get; set; }

    public string CategoryId { get; set; } = null!;
    public Category Category { get; set; } = null!;

    public string PublisherId { get; set; } = null!;
    public Publisher Publisher { get; set; } = null!;

    public ICollection<Author> Authors { get; set; } = null!;
    public ICollection<Borrow> Borrows { get; set; } = null!;
}