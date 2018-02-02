using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class EligibilityController : DefaultController
    {
        private const string CurrentPaEmail = "CURRENT_PA_EMAIL";

        private readonly ISessionHelper session;
        private readonly ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder;
        private readonly ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler;
        private readonly IConstantService constantService;

        public EligibilityController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder, IFormDefinition formDefinition,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler, IConstantService constantService)
            : base(formDefinition)
        {
            this.session = session;
            this.licenceApplicationViewModelBuilder = licenceApplicationViewModelBuilder;
            this.licenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
            this.constantService = constantService;
        }

        protected override string GetViewPath(FormSection section, int id)
        {
            return $"{section.ToString()}.{id}";
        }

        [HttpGet]
        [ImportModelState]
        [Route("Eligibility/Part/{id}")]
        public ActionResult Eligibility(int id)
        {
            var licenceId = session.GetCurrentLicenceId();

            var model = licenceApplicationViewModelBuilder.Build<EligibilityViewModel>(licenceId);

            return GetNextView(id, FormSection.Eligibility, model);            
        }

        [HttpGet]
        public ActionResult Introduction()
        {
            return View("Introduction");
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult Introduction(LicenceApplicationViewModel model)
        {
            return RedirectToAction("Eligibility", 1);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveEmailAddress(PrincipalAuthorityEmailAddressViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part1");
            session.SetInt("LastSubmittedPageId", 1);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.1", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("Eligibility", 2);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveFullName(PrincipalAuthorityFullNameViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part2");
            session.SetInt("LastSubmittedPageId", 2);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.2", model);
            }

            var licenceId = session.GetCurrentLicenceId();

            licenceApplicationPostDataHandler.Update(licenceId, x => x, model);

            return RedirectToAction("Eligibility", 3);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveAddress(AddressViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part3");
            session.SetInt("LastSubmittedPageId", 3);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.3", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("Eligibility", 4);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part4");
            session.SetInt("LastSubmittedPageId", 4);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.4", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("Eligibility", 5);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SavePassword(PasswordViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part4");
            session.SetInt("LastSubmittedPageId", 5);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.5", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("Eligibility", 6);
        }
    }
}