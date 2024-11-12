using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Controllers
{
    public class ProductController : Controller
    {
        private readonly StorageService _storageService;

        public ProductController(StorageService storageService)
        {
            _storageService = storageService;
        }

        // List blobs in a container
        [HttpGet]
        public async Task<IActionResult> ListBlobs(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
            {
                return BadRequest("Container name is required.");
            }

            var blobs = await _storageService.ListBlobsAsync(containerName);
            return View(blobs); // Ensure you have a view set up to display the blobs
        }

        // Upload a blob
        [HttpPost]
        public async Task<IActionResult> UploadBlob(string containerName, string blobName, Stream fileStream)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            await _storageService.UploadBlobAsync(containerName, blobName, fileStream);
            return Ok("Blob uploaded successfully.");
        }
    }
}
