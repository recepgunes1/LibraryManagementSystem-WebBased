using System.ComponentModel.DataAnnotations;

namespace LMS.Entity.ViewModels.Publisher;

public class UpdatePublisherViewModel
{    
    [Required]
    public string Id { get; set; } = null!;
    
    [Required]
    public string Name { get; set; } = null!;
    public string? BackStory { get; set; }
}