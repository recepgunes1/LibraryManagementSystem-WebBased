using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.Author;

public class UpdateAuthorViewModel
{
    [Required]
    public string Id { get; set; } = null!;
    
    [Required]
    public string FullName { get; set; } = null!;
    public string? BackStory { get; set; }
}