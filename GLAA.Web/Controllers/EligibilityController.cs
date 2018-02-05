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

        protected override ActionResult RedirectToAction(FormSection section, int id)
        {
            return RedirectToAction(section.ToString(), new { id });
        }

        protected override ActionResult RedirectBackToAction(FormSection section, int id)
        {
            return RedirectToAction(section.ToString(), new { id, back = true });
        }

        protected override ActionResult RedirectToLastAction(FormSection section)
        {
            return RedirectToAction(section.ToString(), new { id = FormDefinition.GetSectionLength(section) });
        }

        protected IActionResult CheckParentValidityAndRedirect(int submittedPageId)
        {
            var email = session.GetString(CurrentPaEmail);
            var sectionLength = FormDefinition.GetSectionLength(FormSection.Eligibility);
            var nextPageId = submittedPageId + 1;

            if (submittedPageId + 1 != sectionLength)
            {
                var parent = accountCreationViewModelBuilder.Build(email);

                return parent == null
                    ? RedirectToAction("Introduction", "Eligibility")
                    : ValidateParentAndRedirect(parent, nextPageId);
            }

            return RedirectToLastAction(FormSection.Eligibility);
        }

        protected IActionResult CheckParentValidityAndRedirectBack(int submittedPageId)
        {
            var email = session.GetString(CurrentPaEmail);
            var nextPageId = submittedPageId - 1;
            var parent = accountCreationViewModelBuilder.Build(email);

            return parent == null && nextPageId > 0
                ? RedirectBackToAction(FormSection.Eligibility, nextPageId)
                : ValidateParentAndRedirectBack(parent, nextPageId);
        }

        protected IActionResult ValidateParentAndRedirect(EligibilityViewModel parent, int nextPageId)
        {
            parent.Validate();
            return parent.IsValid
                ? RedirectToLastAction(FormSection.Eligibility)
                : RedirectToAction(FormSection.Eligibility, nextPageId);
        }

        protected IActionResult ValidateParentAndRedirectBack(EligibilityViewModel parent, int nextPageId)
        {
            parent.Validate();

            return nextPageId > 0
                ? (parent.IsValid
                    ? RedirectToLastAction(FormSection.Eligibility)
                    : RedirectBackToAction(FormSection.Eligibility, nextPageId))
                : RedirectToLastAction(FormSection.Eligibility);
        }

        [HttpGet]
        [ImportModelState]
        [Route("Eligibility/Part/{id}")]
        public ActionResult Eligibility(int id, bool? back = false)
        {
            session.SetLoadedPage(id);
            var email = session.GetString(CurrentPaEmail);

            var model = accountCreationViewModelBuilder.Build(email);

            return back.HasValue && back.Value
                ? GetPreviousView(id, FormSection.Eligibility, model)
                : GetNextView(id, FormSection.Eligibility, model);
        }

        public IActionResult Back(int submittedPageId)
        {
            return CheckParentValidityAndRedirectBack(submittedPageId);
        }

        [HttpGet]
        public ActionResult Introduction()
        {
            return View("Introduction");
        }

        [HttpGet]
        public ActionResult VerificationSent()
        {
            return View("VerificationSent");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveEmailAddress(PrincipalAuthorityEmailAddressViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 1);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Eligibility, 1), model);
            }

            session.SetString(CurrentPaEmail, model.EmailAddress);

            accountCreationPostDataHandler.Update(model.EmailAddress, model);

            return CheckParentValidityAndRedirect(1);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveFullName(PrincipalAuthorityFullNameViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 2);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Eligibility, 2), model);
            }

            accountCreationPostDataHandler.Update(session.GetString(CurrentPaEmail), model);

            return CheckParentValidityAndRedirect(2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAddress(AddressViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 3);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Eligibility, 3), model);
            }

            accountCreationPostDataHandler.UpdateAddress(session.GetString(CurrentPaEmail), model);

            return CheckParentValidityAndRedirect(3);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 4);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Eligibility, 4), model);
            }

            accountCreationPostDataHandler.Update(session.GetString(CurrentPaEmail), model);

            return CheckParentValidityAndRedirect(4);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePassword(PasswordViewModel model)
        {
            session.SetSubmittedPage(FormSection.Eligibility, 5);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Eligibility, 5), model);
            }

            accountCreationPostDataHandler.SetPassword(session.GetString(CurrentPaEmail), model.Password);

            return CheckParentValidityAndRedirect(5);
        }
        
        [HttpPost]
        [ExportModelState]
        public ActionResult SendVerification(EligibilityViewModel model)
        {
            return RedirectToAction("VerificationSent");
        }
    }
}