using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;

namespace SonicSpectrum.Application.Services
{
    public class UploadFileHelper
    {
        public async static Task<string> UploadFile(IFormFile file,string containerName, string id)
        {
            string constr = @"DefaultEndpointsProtocol=https;AccountName=seventysoundst;AccountKey=ZSBJrnqHp9LVyiE132TSdJBXB5GKu4i5ob0nj73Xa41VeZEe73e09C0ftksLS522XEM0PTWPSi1W+AStYWkuag==;EndpointSuffix=core.windows.net";
            BlobContainerClient blobContainerClient = new BlobContainerClient(constr, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(id);
            var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            await blobClient.UploadAsync(memoryStream);
            var path = blobClient.Uri.AbsoluteUri;
            return path;
        }

        public async static Task DeleteFile(string id, string containerName)
        {
            string constr = @"DefaultEndpointsProtocol=https;AccountName=seventysoundst;AccountKey=ZSBJrnqHp9LVyiE132TSdJBXB5GKu4i5ob0nj73Xa41VeZEe73e09C0ftksLS522XEM0PTWPSi1W+AStYWkuag==;EndpointSuffix=core.windows.net";
            BlobContainerClient blobContainerClient = new BlobContainerClient(constr, containerName);
            BlobClient blobClient = blobContainerClient.GetBlobClient(id);
            await blobClient.DeleteAsync();
        }
    }
}
