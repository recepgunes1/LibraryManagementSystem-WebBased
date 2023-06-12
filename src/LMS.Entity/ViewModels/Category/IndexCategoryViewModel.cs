namespace LMS.Entity.ViewModels.Category;

public class IndexCategoryViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? BackStory { get; set; }
    public int AmountOfBooks { get; set; }
}