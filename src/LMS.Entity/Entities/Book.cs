using LMS.Core.Entity;

namespace LMS.Entity.Entities;

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

    public string AuthorId { get; set; } = null!;
    public Author Author { get; set; } = null!;

    public string ImageId { get; set; } = null!;
    public Image Image { get; set; } = null!;

    public ICollection<Borrow> Borrows { get; set; } = null!;
}