using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pspcl.Services.Interfaces
{
    public interface IBlobStorageService
    {
        string UploadImageToAzure(IFormFile file);
        public MemoryStream DownloadImage(string fileName);
        public string GetContentType(string fileExtension);
    }
}
