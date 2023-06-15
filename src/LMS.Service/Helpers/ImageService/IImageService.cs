using LMS.Entity.Entities;
using Microsoft.AspNetCore.Http;

namespace LMS.Service.Helpers.ImageService;

public interface IImageService
{
    Task<Image> UploadAsync(IFormFile image, string folder);
    void Delete(string file);
}