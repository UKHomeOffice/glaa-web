using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GLAA.ViewModels.File
{
    public class FileUploadViewModel
    {
        [Required]
        public IFormFile FormFileUpload { get; set; }
    }
}
