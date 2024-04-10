using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace SonicSpectrum.Application.Services
{
    public class UploadFileHelper
    {
        public async static Task<string> UploadFile(IFormFile file,string containerName, string fileName)
        {
            string constr = @"DefaultEndpointsProtocol=https;AccountName=musicstrgac;AccountKey=s0U5pCVjWmtDx6sBwd7QEhYK7NnBdIbwIiosdgp74IiioLyYL+oefOAbKEKgFPMCWVfjHmLa/xr8+ASte62Wbw==;EndpointSuffix=core.windows.net";
            BlobContainerClient blobContainerClient = new BlobContainerClient(constr, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            var path = blobClient.Uri.AbsoluteUri;
            return path;
        }

        public async static Task DeleteFile(string fileName, string containerName)
        {
            string constr = @"DefaultEndpointsProtocol=https;AccountName=musicstrgac;AccountKey=s0U5pCVjWmtDx6sBwd7QEhYK7NnBdIbwIiosdgp74IiioLyYL+oefOAbKEKgFPMCWVfjHmLa/xr8+ASte62Wbw==;EndpointSuffix=core.windows.net";
            BlobContainerClient blobContainerClient = new BlobContainerClient(constr, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }
    }
}
