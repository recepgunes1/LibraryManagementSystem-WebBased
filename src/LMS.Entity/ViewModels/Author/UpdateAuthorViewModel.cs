namespace LMS.Entity.ViewModels.Author;

public class UpdateAuthorViewModel
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? BackStory { get; set; }
}