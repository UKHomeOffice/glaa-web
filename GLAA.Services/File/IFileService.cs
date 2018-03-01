using System.Threading.Tasks;
using GLAA.ViewModels.File;

namespace GLAA.Services.File
{
    public interface IFileService
    {
        Task<FileUploadedViewModel> UploadFile(FileUploadViewModel fileUploadViewModel);
        Task<FileSummaryViewModel> GetFileSummary(string key);
    }
}