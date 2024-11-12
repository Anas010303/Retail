using ABC_Retail.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Controllers
{
    public class FileController : Controller
    {
        private readonly StorageService _storageService;

        public FileController(StorageService storageService)
        {
            _storageService = storageService;
        }

        // List files in Azure File Share
        [HttpGet]
        public async Task<IActionResult> ListFiles(string shareName, string directoryName)
        {
            if (string.IsNullOrEmpty(shareName) || string.IsNullOrEmpty(directoryName))
            {
                return BadRequest("Share name and directory name are required.");
            }

            var files = await _storageService.ListFilesAsync(shareName, directoryName);
            return View(files); // Ensure you have a corresponding view set up to display the files
        }

        // Upload a file to Azure File Share
        [HttpPost]
        public async Task<IActionResult> UploadFile(string shareName, string directoryName, string fileName, Stream fileStream)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                return BadRequest("File is empty.");
            }

            await _storageService.UploadFileAsync(shareName, directoryName, fileName, fileStream);
            return Ok("File uploaded successfully.");
        }
    }
}
