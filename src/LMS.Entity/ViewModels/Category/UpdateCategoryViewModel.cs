namespace LMS.Entity.ViewModels.Category;

public class UpdateCategoryViewModel
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? BackStory { get; set; }
    public Dictionary<string, string>? Parents { get; set; }
    public string? ParentCategoryId { get; set; }
}