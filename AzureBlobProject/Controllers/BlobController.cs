using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureBlobProject.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AzureBlobProject.Controllers
{
    public class BlobController : Controller
    {
        private readonly IContainerService _containerService;
        private readonly IBlobService _blobService;
        public BlobController(IContainerService containerService, IBlobService blobService)
        {
            _containerService = containerService;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _containerService.GetContainers();
            return View();
        }

        public async Task<JsonResult> Containers()
        {
            var data = await _containerService.GetContainers();
            return Json(data);
        }

        public async Task<JsonResult> ContainerFolders(string containerName)
        {
            var data = await _containerService.GetContainerFolders(containerName);
            return Json(data);
        }

        public async Task<JsonResult> FolderFiles(string containerName, string folderName)
        {
            var data = await _containerService.GetFolderFiles(containerName, folderName);
            return Json(data);
        }

        public async Task<JsonResult> BlobList(string containerName)
        {
            var data = await _blobService.GetBlobs(containerName);
            return Json(data);
        }

        public JsonResult Blob(string name, string containerName)
        {
            var data = _blobService.GetBlob(name, containerName);
            return Json(data);
        }

        public async Task<JsonResult> UploadBlob(string name, IFormFile file, string containerName)
        {
            var data = await _blobService.UploadBlob(name, file, containerName);
            return Json(data);
        }

        public async Task<JsonResult> DeleteBlob(string name, string containerName)
        {
            await _blobService.DeleteBlob(name, containerName);
            return Json("");
        }

        public async Task<JsonResult> GetBlobBase64(string name, string containerName)
        {
            byte[] bytes = await _blobService.DownloadBlob(name, containerName);
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);

            return Json(base64String);
        }
    }
}
