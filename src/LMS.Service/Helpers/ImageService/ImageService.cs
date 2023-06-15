using LMS.Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace LMS.Service.Helpers.ImageService;

public class ImageService : IImageService
{
    private readonly string _wwwroot;
    private const string ImgFolder = "images";

    public ImageService(IHostingEnvironment environment)
    {
        _wwwroot = environment.WebRootPath;
    }
    
    public async Task<Image> UploadAsync(IFormFile image, string folder)
    {
        var folderPath = Path.Combine(_wwwroot, ImgFolder, folder);
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var extension = Path.GetExtension(image.FileName).ToLower();
        var newFileName = $"{Guid.NewGuid()}{extension}";
        var imagePath = Path.Combine(folderPath, newFileName);
        await using FileStream stream = new(imagePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 124,
            useAsync: false);
        await image.CopyToAsync(stream);
        await stream.FlushAsync();
        return new Image { FileName = newFileName, FolderName = folder };
    }

    public void Delete(string file)
    {
        var filePath = Path.Combine(_wwwroot, file.Remove(0,1));
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}