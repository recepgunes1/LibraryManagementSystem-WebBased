namespace LMS.Entity.ViewModels.Publisher;

public class IndexPublisherViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? BackStory { get; set; }
    public int AmountOfBooks { get; set; }
}