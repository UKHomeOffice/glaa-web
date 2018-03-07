using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace GLAA.ViewModels.File
{
    public class FileUploadViewModel
    {
        [Required]
        [Display(Name = "Add a File", Description = "Please select a file to add to the upload.")]
        public IFormFile FormFileUpload { get; set; }
        public int LicenceId { get; set; }
    }
}
