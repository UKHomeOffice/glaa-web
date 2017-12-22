using GLAA.Web.FormLogic;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class HomeController : DefaultController
    {
        public HomeController(IFormDefinition formDefinition)
            : base(formDefinition)
        {
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
    }
}