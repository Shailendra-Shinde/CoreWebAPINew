using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Polly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebAPI.Interfaces
{
    public interface IFileUpload
    {
        Task CreateContainer(Guid guid);

        Task UploadFile(Guid batchId, string fileName, string localFilePath);
    }

    public class AzureSender : IFileUpload
    {
        public AzureSender(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async Task CreateContainer(Guid guid)
        {
            var connString = Configuration.GetValue<string>("StorageConnectionString");
            var retryPolicy = Policy.Handle<StorageException>()
                .RetryAsync(2, async (ex, count, context) =>
                {
                    (Configuration as IConfigurationRoot).Reload();
                    connString = Configuration.GetValue<string>("StorageConnectionString");
                });
            await retryPolicy.ExecuteAsync(() => CreateContainerInstance(guid, connString));
        }

        public static async Task CreateContainerInstance(Guid guid, string connString)
        {
            var storageAccount = CloudStorageAccount.Parse(connString);
            storageAccount.CreateCloudBlobClient();

            BlobContainerClient container = new BlobContainerClient(connString, guid.ToString().ToLower());
            container.CreateIfNotExists(PublicAccessType.Blob);

            await container.CreateIfNotExistsAsync();
        }

        public async Task UploadFile(Guid batchId, string fileName, string localFilePath)
        {
            var connString = Configuration.GetValue<string>("StorageConnectionString");
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(connString);

            //Create a unique name for the container
            string containerName = batchId.ToString().ToLower();

            // Create the container and return a container client object
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

            // Open the file and upload its data
            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream, true);
            uploadFileStream.Close();
        }
    }
}
