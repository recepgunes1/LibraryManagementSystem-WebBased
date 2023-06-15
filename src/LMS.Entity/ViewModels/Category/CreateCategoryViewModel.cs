using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.Category;

public class CreateCategoryViewModel
{
    [Required]
    public string Name { get; set; } = null!;
    public string? BackStory { get; set; }
}