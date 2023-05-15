namespace LMS.Entity.ViewModels.Book;

public class UpdateBookViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public int Pages { get; set; }
    public int Amount { get; set; }
    public DateTime PublishedDateTime { get; set; }
    public string CategoryId { get; set; } = null!;
    public string PublisherId { get; set; } = null!;
}