using GLAA.Services.AccountCreation;
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
        private readonly IAccountCreationViewModelBuilder accountCreationViewModelBuilder;
        private readonly IAccountCreationPostDataHandler accountCreationPostDataHandler;

        public EligibilityController(ISessionHelper session,
            IAccountCreationViewModelBuilder accountCreationViewModelBuilder, IFormDefinition formDefinition,
            IAccountCreationPostDataHandler accountCreationPostDataHandler)
            : base(formDefinition)
        {
            this.session = session;
            this.accountCreationViewModelBuilder = accountCreationViewModelBuilder;
            this.accountCreationPostDataHandler = accountCreationPostDataHandler;
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
            var email = session.GetString(CurrentPaEmail);

            var model = accountCreationViewModelBuilder.Build(email);

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
            session.SetSubmittedPage(FormSection.Eligibility, 1);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.1", model);
            }

            session.SetString(CurrentPaEmail, model.EmailAddress);

            accountCreationPostDataHandler.Update(model.EmailAddress, model);

            return RedirectToAction("Eligibility", 2);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveFullName(PrincipalAuthorityFullNameViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 2);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.2", model);
            }

            accountCreationPostDataHandler.Update(session.GetString(CurrentPaEmail), model);

            return RedirectToAction("Eligibility", 3);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveAddress(AddressViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 3);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.3", model);
            }

            accountCreationPostDataHandler.UpdateAddress(session.GetString(CurrentPaEmail), model);

            return RedirectToAction("Eligibility", 4);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SaveCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 4);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.4", model);
            }

            accountCreationPostDataHandler.Update(session.GetString(CurrentPaEmail), model);

            return RedirectToAction("Eligibility", 5);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult SavePassword(PasswordViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 5);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.5", model);
            }

            accountCreationPostDataHandler.Update(session.GetString(CurrentPaEmail), model);

            return RedirectToAction("Eligibility", 6);
        }
    }
}