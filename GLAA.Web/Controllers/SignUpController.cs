using System;
using GLAA.Services;
using GLAA.Services.AccountCreation;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GLAA.Web.Controllers
{
    public class SignUpController : DefaultController
    {
        private const string CurrentPaEmail = "CURRENT_PA_EMAIL";

        private readonly ISessionHelper session;
        private readonly IAccountCreationViewModelBuilder accountCreationViewModelBuilder;
        private readonly IAccountCreationPostDataHandler accountCreationPostDataHandler;

        public SignUpController(ISessionHelper session,
            IAccountCreationViewModelBuilder accountCreationViewModelBuilder, IFormDefinition formDefinition,
            IAccountCreationPostDataHandler accountCreationPostDataHandler, IReferenceDataProvider rdp)
            : base(formDefinition, rdp)
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
            var sectionLength = FormDefinition.GetSectionLength(FormSection.SignUp);
            var nextPageId = submittedPageId + 1;

            if (submittedPageId + 1 != sectionLength)
            {
                var parent = accountCreationViewModelBuilder.Build(email);

                return parent == null
                    ? RedirectToAction("Introduction", "SignUp")
                    : ValidateParentAndRedirect(parent, nextPageId);
            }

            return RedirectToLastAction(FormSection.SignUp);
        }

        protected IActionResult CheckParentValidityAndRedirectBack(int submittedPageId)
        {
            var email = session.GetString(CurrentPaEmail);
            var nextPageId = submittedPageId - 1;
            var parent = accountCreationViewModelBuilder.Build(email);

            return parent == null && nextPageId > 0
                ? RedirectBackToAction(FormSection.SignUp, nextPageId)
                : ValidateParentAndRedirectBack(parent, nextPageId);
        }

        protected IActionResult ValidateParentAndRedirect(SignUpViewModel parent, int nextPageId)
        {
            parent.Validate();
            return parent.IsValid
                ? RedirectToLastAction(FormSection.SignUp)
                : RedirectToAction(FormSection.SignUp, nextPageId);
        }

        protected IActionResult ValidateParentAndRedirectBack(SignUpViewModel parent, int nextPageId)
        {
            parent.Validate();

            return nextPageId > 0
                ? (parent.IsValid
                    ? RedirectToLastAction(FormSection.SignUp)
                    : RedirectBackToAction(FormSection.SignUp, nextPageId))
                : RedirectToLastAction(FormSection.SignUp);
        }

        [HttpGet]
        [ImportModelState]
        [Route("SignUp/Part/{id}")]
        public ActionResult SignUp(int id, bool? back = false)
        {
            session.SetLoadedPage(id);

            var email = id != 1 ? session.GetString(CurrentPaEmail) : string.Empty;

            var model = accountCreationViewModelBuilder.Build(email);

            return back.HasValue && back.Value
                ? GetPreviousView(id, FormSection.SignUp, model)
                : GetNextView(id, FormSection.SignUp, model);
        }

        public IActionResult Back(int submittedPageId)
        {
            return CheckParentValidityAndRedirectBack(submittedPageId);
        }

        [HttpGet]
        public ActionResult Introduction()
        {
            return View(nameof(Introduction));
        }

        [HttpGet]
        public ActionResult VerificationSent()
        {
            return View(nameof(VerificationSent), session.GetString(CurrentPaEmail));
        }

        [HttpGet]
        public async Task<IActionResult> ResendVerification()
        {
            await accountCreationPostDataHandler.SendConfirmationAsync(session.GetString(CurrentPaEmail), Url);
            return RedirectToAction(nameof(VerificationSent));
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> SaveEmailAddress(PrincipalAuthorityEmailAddressViewModel model)
        {
            session.SetSubmittedPage(FormSection.SignUp, 1);

            if (await accountCreationPostDataHandler.ExistsAsync(model.EmailAddress))
            {
                ViewData["doOverride"] = true;
                ModelState.AddModelError("EmailAddress", "A user with this email address already exists in the system.");
            }

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.SignUp, 1), model);
            }

            // Don't overwrite an unconfirmed user if we're currently editing that user
            if (!model.EmailAddress.Equals(session.GetString(CurrentPaEmail),
                StringComparison.InvariantCultureIgnoreCase))
            {
                await accountCreationPostDataHandler.DeleteIfUnconfirmedAsync(model.EmailAddress);
            }

            session.SetString(CurrentPaEmail, model.EmailAddress);

            await accountCreationPostDataHandler.UpdateAsync(model.EmailAddress, model);

            return CheckParentValidityAndRedirect(1);
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> SaveFullName(PrincipalAuthorityFullNameViewModel model)
        {
            session.SetSubmittedPage(FormSection.SignUp, 2);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.SignUp, 2), model);
            }

            await accountCreationPostDataHandler.UpdateAsync(session.GetString(CurrentPaEmail), model);

            return CheckParentValidityAndRedirect(2);
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> SaveAddress(AddressViewModel model)
        {
            session.SetSubmittedPage(FormSection.SignUp, 3);

            if (!ModelState.IsValid)
            {
                // have to repopulate dropdowns as lost during post
                model.Countries = ReferenceDataProvider.GetCountries();
                model.Counties = ReferenceDataProvider.GetCounties();

                return View(GetViewPath(FormSection.SignUp, 3), model);
            }

            await accountCreationPostDataHandler.UpdateAddressAsync(session.GetString(CurrentPaEmail), model);

            return CheckParentValidityAndRedirect(3);
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> SaveCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            session.SetSubmittedPage(FormSection.SignUp, 4);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.SignUp, 4), model);
            }

            await accountCreationPostDataHandler.UpdateAsync(session.GetString(CurrentPaEmail), model);

            return CheckParentValidityAndRedirect(4);
        }

        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> SavePassword(PasswordViewModel model)
        {
            session.SetSubmittedPage(FormSection.SignUp, 5);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.SignUp, 5), model);
            }

            var passwordSetResult = await accountCreationPostDataHandler.SetPasswordAsync(session.GetString(CurrentPaEmail), model.Password);

            return CheckParentValidityAndRedirect(5);
        }
        
        [HttpPost]
        [ExportModelState]
        public async Task<IActionResult> SendVerification()
        {
            await accountCreationPostDataHandler.SendConfirmationAsync(session.GetString(CurrentPaEmail), Url);
            return RedirectToAction(nameof(VerificationSent));
        }
    }
}