namespace LMS.Entity.ViewModels.Author;

public class IndexAuthorViewModel
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? BackStory { get; set; }
    public int AmountOfBooks { get; set; }
}