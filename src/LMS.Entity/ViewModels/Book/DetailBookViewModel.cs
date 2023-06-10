//I need to show following view model in my view. I use Bootstrap. Can you create a view for me?

namespace LMS.Entity.ViewModels.Book;

public class DetailBookViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Isbn { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public int Pages { get; set; }
    public int Amount { get; set; }
    public DateTime PublishedDateTime { get; set; }
    public string Author { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string Publisher { get; set; } = null!;
}