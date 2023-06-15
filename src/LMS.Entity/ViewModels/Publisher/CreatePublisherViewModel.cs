using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.Publisher;

public class CreatePublisherViewModel
{
    [Required]
    public string Name { get; set; } = null!;
    
    public string? BackStory { get; set; }
}