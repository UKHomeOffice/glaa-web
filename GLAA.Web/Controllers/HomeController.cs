using GLAA.Services;
using GLAA.Web.FormLogic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GLAA.Web.Controllers
{
    public class HomeController : DefaultController
    {
        private readonly IFileUploadService fileUploadService;

        public HomeController(IFormDefinition formDefinition, IFileUploadService fileUploadService)
            : base(formDefinition)
        {
            this.fileUploadService = fileUploadService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IActionResult SessionTimeout()
        {
            return View("SessionTimeout");
        }

        public async Task UploadFile()
        {
            using (var file = System.IO.File.OpenRead("wwwroot/images/robot-hand.png"))
            {
                await fileUploadService.UploadFile(file);
            }                            
        }
    }
}