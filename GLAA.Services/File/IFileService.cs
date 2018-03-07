using System.Threading.Tasks;
using GLAA.ViewModels.File;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Services.File
{
    public interface IFileService
    {
        Task<FileUploadedViewModel> UploadFile(FileUploadViewModel fileUploadViewModel);
        FileSummaryViewModel GetFileSummary(string key);
        Task<FileStreamResult> GetFilePreviewImage(string key);
    }
}