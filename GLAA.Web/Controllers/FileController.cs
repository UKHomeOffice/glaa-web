using GLAA.Services.File;
using GLAA.ViewModels.File;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class FileController : Controller
    {
        public IFileService fileService;

        public FileController(IFileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet]
        public IActionResult Index(string id)
        {
            //TODO - Get currently staged files for upload.
            if (!string.IsNullOrWhiteSpace(id))
            {
                var fileSummaryViewModel = fileService.GetFileSummary(id);

                return View(fileSummaryViewModel);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult FileReview(string key)
        {
            //TODO - Get currently staged files for upload.
            if (!string.IsNullOrWhiteSpace(key))
            {
                var fileSummaryViewModel = fileService.GetFileSummary(key);

                return View(fileSummaryViewModel);
            }

            return View();
        }

        [HttpGet]
        public IActionResult FilePreviewImage(string key)
        {
            //TODO - Get currently staged files for upload.
            if (!string.IsNullOrWhiteSpace(key))
                return fileService.GetFilePreviewImage(key).Result;

            return new NotFoundResult();
        }

        [HttpPost]
        public IActionResult Index(FileUploadViewModel fileUploadViewModel)
        {
            var fileUploadedViewModel = fileService.UploadFile(fileUploadViewModel).Result;

            return RedirectToAction("FileReview", new { key = fileUploadedViewModel.Key });
        }

        [HttpPost]
        public IActionResult Confirm(FileSummaryViewModel fileSummaryViewModel)
        {
            if (fileSummaryViewModel.CorrectFile)
            {
                //TODO - handle redirect to the file collection summary.    
            }
            else
                return Index("");

            return null;
        }
    }
}