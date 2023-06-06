namespace LMS.Entity.ViewModels.Category;

public class CreateCategoryViewModel
{
    public string Name { get; set; } = null!;
    public string? BackStory { get; set; }
    public Dictionary<string, string>? Parents { get; set; }
    public string? ParentCategoryId { get; set; }
}