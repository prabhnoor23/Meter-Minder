using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Pspcl.Services.Options;
using Microsoft.Extensions.Configuration;
using Pspcl.Services.Interfaces;
using Microsoft.Extensions.Options;
using Pspcl.DBConnect;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace Pspcl.Services
{
    public class BlobStorageService: IBlobStorageService
    {
        private readonly AzureOptions _azureOptions;
     
        public BlobStorageService(IOptions<AzureOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;
        }
        public string UploadImageToAzure(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName);
                string contentType = GetContentType(fileExtension);

                using MemoryStream fileUploadStream = new MemoryStream();
            {
                file.CopyTo(fileUploadStream);
                fileUploadStream.Position = 0;
                BlobContainerClient blobContainerClient = new BlobContainerClient(
                    _azureOptions.ConnectionString,
                    _azureOptions.Container);

                var uniqueName = Guid.NewGuid().ToString() + fileExtension;

                try
                {
                    //azure package is required for below line
                    BlobClient blobClient = blobContainerClient.GetBlobClient(uniqueName);
                    blobClient.Upload(fileUploadStream, new BlobUploadOptions()
                    {
                        HttpHeaders = new BlobHttpHeaders
                        {
                            ContentType = contentType
                        }
                    }, cancellationToken: default);
                    return uniqueName;
                }
                catch (RequestFailedException ex)
                {
                    // An exception occurred during the upload
                    Console.WriteLine("Upload failed. Error message: " + ex.Message);
                    return "failure";
                }
            }

        }
        public MemoryStream DownloadImage( string fileName)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(_azureOptions.ConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_azureOptions.Container);
            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            // Download the blob contents to a memory stream
            MemoryStream memoryStream = new MemoryStream();
            blobClient.DownloadTo(memoryStream);
            memoryStream.Position = 0;

            // Set the response content type based on the file extension
            string contentType = GetContentType(fileName);
            if (contentType == null)
                contentType = "application/octet-stream"; // Default content type if unknown
            return memoryStream;            

        }

        public string GetContentType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }

    }
}
