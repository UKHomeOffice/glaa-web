using System;
using System.Linq;
using GLAA.Common;
using GLAA.Domain.Models;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class PrincipalAuthorityController : LicenceApplicationBaseController
    {
        public PrincipalAuthorityController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService, IReferenceDataProvider rdp) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, rdp)
        {
        }

        private PrincipalAuthorityViewModel SetupGetPart(string actionName)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                x => x.PrincipalAuthorities.FirstOrDefault());

            if (model.Id.HasValue)
                Session.SetCurrentPaStatus(model.Id.Value, model.IsDirector.IsDirector ?? false);

            if (model.DirectorOrPartnerId.HasValue)
                Session.SetCurrentDopStatus(model.DirectorOrPartnerId.Value, model.IsDirector.IsDirector ?? false);

            Session.SetLoadedPage(actionName);
            return model;
        }

        private IActionResult PrincipalAuthorityGet(string actionName, bool? back = false)
        {
            var model = SetupGetPart(actionName);

            return
                back.HasValue && back.Value
                    ? GetPreviousView(FormSection.PrincipalAuthority, actionName, model)
                    : GetNextView(FormSection.PrincipalAuthority, actionName, model);
        }


        private IActionResult PrincipalAuthorityPost<T>(T model, string submittedPage, bool doDopLinking = true)
        {
            return PrincipalAuthorityPost(model, submittedPage, doDopLinking, m => !ModelState.IsValid);
        }

        private IActionResult PrincipalAuthorityPost<T>(T model, string submittedPage, bool doDopLinking, Func<T, bool> modelIsInvalid)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, submittedPage);

            model = RepopulateDropdowns(model);

            if (modelIsInvalid(model))
            {
                return View(submittedPage, model);
            }

            if (Session.GetCurrentPaIsDirector() && doDopLinking)
            {
                LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.DirectorOrPartners, model,
                    Session.GetCurrentDopId());
            }

            var paId = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.PrincipalAuthorities, model,
                Session.GetCurrentPaId());

            Session.SetCurrentPaStatus(paId, doDopLinking);

            return CheckParentValidityAndRedirect(FormSection.PrincipalAuthority, submittedPage);
        }

        public IActionResult Introduction(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(Introduction), back);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult IsDirector(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(IsDirector), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult IsDirector(IsDirectorViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, nameof(IsDirector));

            if (!ModelState.IsValid || !model.IsDirector.HasValue)
            {
                return View(nameof(IsDirector), model);
            }

            var paId = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.PrincipalAuthorities,
                model, Session.GetCurrentPaId());

            Session.SetCurrentPaStatus(paId, model.IsDirector.Value);

            if (model.IsDirector.Value)
            {
                var licenceModel = LicenceApplicationViewModelBuilder.Build(Session.GetCurrentLicenceId());

                var existingDirectorOrPartnerId = licenceModel.PrincipalAuthority.DirectorOrPartnerId ?? 0;

                var dopId = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.DirectorOrPartners,
                    model,
                    existingDirectorOrPartnerId);

                Session.SetCurrentDopStatus(dopId, true);

                LicenceApplicationPostDataHandler.UpsertDirectorOrPartnerAndLinkToPrincipalAuthority(
                    Session.GetCurrentLicenceId(), paId, dopId, model);

                return RedirectToAction(FormSection.PrincipalAuthority, nameof(FullName));
            }

            LicenceApplicationPostDataHandler.UnlinkDirectorOrPartnerFromPrincipalAuthority(Session.GetCurrentPaId());

            return CheckParentValidityAndRedirect(FormSection.PrincipalAuthority, nameof(IsDirector));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PrincipalAuthorityConfirmation(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(PrincipalAuthorityConfirmation), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PrincipalAuthorityConfirmation(PrincipalAuthorityConfirmationViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(PrincipalAuthorityConfirmation), false, m => !ModelState.IsValid || !m.WillProvideConfirmation);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult FullName(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(FullName), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult FullName(FullNameViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(FullName));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult AlternativeName(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(AlternativeName), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AlternativeName(AlternativeFullNameViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(AlternativeName));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult DateOfBirth(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(DateOfBirth), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DateOfBirth(DateOfBirthViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(DateOfBirth));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult JobTitle(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(JobTitle), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult JobTitle(JobTitleViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(JobTitle));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Address(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(Address), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Address(AddressViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, nameof(Address));

            if (!ModelState.IsValid)
            {
                return View(nameof(Address), model);
            }

            if (Session.GetCurrentPaIsDirector())
            {
                LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(),
                    l => l.DirectorOrPartners.Single(dop => dop.Id == Session.GetCurrentDopId()), model);
            }

            LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(),
                x => x.PrincipalAuthorities.SingleOrDefault(pa => pa.Id == Session.GetCurrentPaId()), model);

            return CheckParentValidityAndRedirect(FormSection.PrincipalAuthority, nameof(Address));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessPhoneNumber(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(BusinessPhoneNumber), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(BusinessPhoneNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessExtension(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(BusinessExtension), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessExtension(BusinessExtensionViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(BusinessExtension));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BirthDetails(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(BirthDetails), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BirthDetails(BirthDetailsViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(BirthDetails));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PersonalMobileNumber(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(PersonalMobileNumber), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PersonalMobileNumber(PersonalMobileNumberViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(PersonalMobileNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PersonalEmailAddress(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(PersonalEmailAddress), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PersonalEmailAddress(PersonalEmailAddressViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(PersonalEmailAddress));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Nationality(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(Nationality), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Nationality(NationalityViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(Nationality));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Passport(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(Passport), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Passport(PassportViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(Passport));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PrincipalAuthorityRightToWork(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(PrincipalAuthorityRightToWork), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PrincipalAuthorityRightToWork(PrincipalAuthorityRightToWorkViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(PrincipalAuthorityRightToWork));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult UndischargedBankrupt(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(UndischargedBankrupt), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult UndischargedBankrupt(UndischargedBankruptViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(UndischargedBankrupt));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult DisqualifiedDirector(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(DisqualifiedDirector), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DisqualifiedDirector(DisqualifiedDirectorViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(DisqualifiedDirector));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult RestraintOrders(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(RestraintOrders), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RestraintOrders(RestraintOrdersViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(RestraintOrders));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult ReviewRestraintOrders(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(ReviewRestraintOrders), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewRestraintOrders(RestraintOrdersViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, nameof(ReviewRestraintOrders));

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
            model = parent.RestraintOrders;

            if ((model.HasRestraintOrders ?? false) && !model.RestraintOrders.Any())
            {
                ModelState.AddModelError(nameof(model.RestraintOrders), "Please enter details of the restraint or confiscation orders or civil recoveries that you have been the subject of.");
                ViewData.Add("doOverride", true);
                return View(nameof(ReviewRestraintOrders), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, nameof(ReviewRestraintOrders));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult UnspentConvictions(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(UnspentConvictions), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult UnspentConvictions(UnspentConvictionsViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(UnspentConvictions));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult ReviewUnspentConvictions(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(ReviewUnspentConvictions), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewUnspentConvictions(UnspentConvictionsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, nameof(ReviewUnspentConvictions));

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
            model = parent.UnspentConvictions;

            if ((model.HasUnspentConvictions ?? false) && !model.UnspentConvictions.Any())
            {
                ModelState.AddModelError(nameof(model.UnspentConvictions), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(nameof(ReviewUnspentConvictions), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, nameof(ReviewUnspentConvictions));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult OffencesAwaitingTrial(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(OffencesAwaitingTrial), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult OffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(OffencesAwaitingTrial));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult ReviewOffencesAwaitingTrial(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(ReviewOffencesAwaitingTrial), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, nameof(ReviewOffencesAwaitingTrial));

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
            model = parent.OffencesAwaitingTrial;

            if ((model.HasOffencesAwaitingTrial ?? false) && !model.OffencesAwaitingTrial.Any())
            {
                ModelState.AddModelError(nameof(model.OffencesAwaitingTrial), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(nameof(ReviewOffencesAwaitingTrial), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, nameof(ReviewOffencesAwaitingTrial));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PreviousLicence(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(PreviousLicence), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PreviousLicence(PreviousLicenceViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(PreviousLicence));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PreviousExperience(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(PreviousExperience), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PreviousExperience(PreviousExperienceViewModel model)
        {
            return PrincipalAuthorityPost(model, nameof(PreviousExperience), false);
        }

        public IActionResult Summary(bool? back = false)
        {
            return PrincipalAuthorityGet(nameof(Summary), back);
        }
    }
}