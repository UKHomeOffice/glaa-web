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
        public IActionResult FileReview(string id)
        {
            //TODO - Get currently staged files for upload.
            if (!string.IsNullOrWhiteSpace(id))
            {
                var fileSummaryViewModel = fileService.GetFileSummary(id).Result;

                return View(fileSummaryViewModel);
            }

            return View();
        }

        [HttpPost]
        public void Index(FileUploadViewModel fileUploadViewModel)
        {
            //TODO - handle file upload.

            var fileUploadedViewModel = fileService.UploadFile(fileUploadViewModel).Result;

            RedirectToAction("FileReview", new { id = fileUploadedViewModel.Key });
        }

        [HttpPost]
        public void Confirm(FileSummaryViewModel fileSummaryViewModel)
        {
            //TODO - associated uploaded files with
        }
    }
}