using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureBlobProject.Services.IServices;

namespace AzureBlobProject.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public ContainerService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetContainerFolders(string containerName)
        {
            Queue<string> prefixes = new Queue<string>();
            prefixes.Enqueue("");
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            List<string> directoryNames = new List<string>();
            do
            {
                string prefix = prefixes.Dequeue();
                await foreach (BlobHierarchyItem blobHierarchyItem in blobContainerClient.GetBlobsByHierarchyAsync(prefix: prefix, delimiter: "/"))
                {
                    if (blobHierarchyItem.IsPrefix)
                    {
                        directoryNames.Add(blobHierarchyItem.Prefix);
                        prefixes.Enqueue(blobHierarchyItem.Prefix);
                    }
                }
            } while (prefixes.Count > 0);
            return directoryNames;
        }

        public async Task<List<string>> GetContainers()
        {
            List<string> containers = new();
            var azureContainers =  _blobServiceClient.GetBlobContainersAsync();
            await foreach(var azureContainer in  azureContainers)
            {
                containers.Add(azureContainer.Name);
            }
            return containers;
        }

        public async Task<List<string>> GetFolderFiles(string containerName, string folderName)
        {
            BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            List<string> files = new();

            var azureFiles = blobContainerClient.GetBlobsAsync(prefix: folderName);
            await foreach (var file in azureFiles)
            {
                files.Add(file.Name);
            }
            return files;
        }
    }
}
