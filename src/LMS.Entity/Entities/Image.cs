using LMS.Core.Entity;

namespace LMS.Entity.Entities;

public class Image : EntityBase
{
    public string FileName { get; set; } = null!;
    public string FolderName { get; set; } = null!;
}