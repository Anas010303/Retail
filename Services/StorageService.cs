using Azure.Storage.Blobs;
using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ABC_Retail.Services
{
    public class StorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly ShareServiceClient _shareServiceClient;
        private readonly QueueServiceClient _queueServiceClient;

        // Constructor to initialize the service clients
        public StorageService(BlobServiceClient blobServiceClient, ShareServiceClient shareServiceClient, QueueServiceClient queueServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _shareServiceClient = shareServiceClient;
            _queueServiceClient = queueServiceClient;
        }

        // Method to upload a blob to Azure Blob Storage
        public async Task UploadBlobAsync(string containerName, string blobName, Stream data)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            await containerClient.UploadBlobAsync(blobName, data);
        }

        // Method to list all blobs in a container
        public async Task<List<string>> ListBlobsAsync(string containerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = new List<string>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobs.Add(blobItem.Name);
            }
            return blobs;
        }

        // Method to add a message to Azure Queue Storage
        public async Task AddMessageToQueueAsync(string queueName, string message)
        {
            var queueClient = _queueServiceClient.GetQueueClient(queueName);
            await queueClient.CreateIfNotExistsAsync();
            await queueClient.SendMessageAsync(message);
        }

        // Method to list files in Azure File Share
        public async Task<List<string>> ListFilesAsync(string shareName, string directoryName)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            var directoryClient = shareClient.GetDirectoryClient(directoryName);
            var fileNames = new List<string>();
            await foreach (var fileItem in directoryClient.GetFilesAndDirectoriesAsync())
            {
                if (!fileItem.IsDirectory)
                {
                    fileNames.Add(fileItem.Name);
                }
            }
            return fileNames;
        }

        // Method to upload a file to Azure File Share
        public async Task UploadFileAsync(string shareName, string directoryName, string fileName, Stream fileStream)
        {
            var shareClient = _shareServiceClient.GetShareClient(shareName);
            var directoryClient = shareClient.GetDirectoryClient(directoryName);
            await directoryClient.CreateIfNotExistsAsync();
            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadRangeAsync(new Azure.HttpRange(0, fileStream.Length), fileStream);
        }
    }
}
