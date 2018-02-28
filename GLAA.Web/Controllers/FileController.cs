using GLAA.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class FileController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            //TODO - Get currently staged files for upload.

            return View();
        }

        [HttpPost]
        public void Index(FileUploadViewModel fileUploadViewModel)
        {
            //TODO - handle file upload.

            RedirectToAction("Index");
        }
    }
}