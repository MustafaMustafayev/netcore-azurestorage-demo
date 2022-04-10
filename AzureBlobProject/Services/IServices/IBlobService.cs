using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AzureBlobProject.Services.IServices
{
    public interface IBlobService
    {
        string GetBlob(string name, string containerName);
        Task<List<string>> GetBlobs(string containerName);
        Task<bool> UploadBlob(string name, IFormFile file, string containerName);
        Task DeleteBlob(string name, string containerName);
        Task<byte[]> DownloadBlob(string blobName, string containerName);
    }
}
