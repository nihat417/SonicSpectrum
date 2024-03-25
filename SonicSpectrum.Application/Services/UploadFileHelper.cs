using Microsoft.AspNetCore.Http;

namespace SonicSpectrum.Application.Services
{
    public class UploadFileHelper
    {
        public async static Task<string> UploadFile(IFormFile file)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string imagePath = $"UploadMusic/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            string path = Path.Combine(currentDirectory, "wwwroot", imagePath);

            FileStream fs = new(path, FileMode.CreateNew, FileAccess.ReadWrite);
            await file.CopyToAsync(fs);
            return "/" + imagePath;
        }
    }
}
