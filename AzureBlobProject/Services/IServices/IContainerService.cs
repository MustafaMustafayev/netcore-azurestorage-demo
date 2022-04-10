using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureBlobProject.Services.IServices
{
    public interface IContainerService
    {
        Task<List<string>> GetContainers();
        Task<List<string>> GetContainerFolders(string containerName);
        Task<List<string>> GetFolderFiles(string containerName, string folderName);
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
    }
}
