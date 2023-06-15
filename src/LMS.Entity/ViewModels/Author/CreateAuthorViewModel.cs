using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.Author;

public class CreateAuthorViewModel
{
    [Required]
    public string FullName { get; set; } = null!;
    public string? BackStory { get; set; }
}