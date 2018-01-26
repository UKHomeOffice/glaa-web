using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GLAA.Services
{
    public interface IFileUploadService
    {
        Task UploadFile(FileStream fileStream);
    }
}
