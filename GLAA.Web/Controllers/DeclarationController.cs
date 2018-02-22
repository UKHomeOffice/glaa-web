using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class DeclarationController : DefaultController
    {
        private readonly ISessionHelper session;
        private readonly ILicenceApplicationViewModelBuilder licenceApplicationViewModel;
        private readonly ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler;

        public DeclarationController(IFormDefinition formDefinition, ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModel,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler, IReferenceDataProvider rdp) : base(
            formDefinition, rdp)
        {
            this.session = session;
            this.licenceApplicationViewModel = licenceApplicationViewModel;
            this.licenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var licenceId = session.GetCurrentLicenceId();

            var model = licenceApplicationViewModel.Build<DeclarationViewModel>(licenceId);

            return View("Index", model);
        }

        [HttpPost]       
        public ActionResult Index(DeclarationViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Index");
            session.SetString("LastSubmittedPageId", "Declaration");

            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("TaskList", "Licence");

        }
    }
}