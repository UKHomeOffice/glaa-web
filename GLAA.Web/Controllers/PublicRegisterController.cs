using GLAA.Services.PublicRegister;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class PublicRegisterController : Controller
    {
        private readonly IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder;

        public PublicRegisterController(IPublicRegisterViewModelBuilder publicRegisterViewModelBuilder)
        {
            this.publicRegisterViewModelBuilder = publicRegisterViewModelBuilder;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var licences = publicRegisterViewModelBuilder.BuildAllLicences();

            return View(licences);
        }

        [HttpGet]
        [Route("PublicRegisterProfile/License/{id}")]
        public IActionResult Licence(int id)
        {
            var licence = publicRegisterViewModelBuilder.BuildLicence(id);


            return View(licence);
        }

    }
}