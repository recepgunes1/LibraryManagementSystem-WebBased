namespace LMS.Entity.ViewModels.Book;

public class CreateBookViewModel
{
    public string Name { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public int Pages { get; set; }
    public int Amount { get; set; }
    public DateTime PublishedDateTime { get; set; }
    public string AuthorId { get; set; } = null!;
    public string CategoryId { get; set; } = null!;
    public string PublisherId { get; set; } = null!;
    
    public Dictionary<string, string>? Authors { get; set; }
    public Dictionary<string, string>? Categories { get; set; }
    public Dictionary<string, string>? Publishers { get; set; }

}