using System;
using System.Linq;
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
            IConstantService constantService) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService)
        {
        }

        private PrincipalAuthorityViewModel SetupGetPart(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                x => x.PrincipalAuthorities.FirstOrDefault());

            if (model.Id.HasValue)
                Session.SetCurrentPaStatus(model.Id.Value, model.IsDirector.IsDirector ?? false);

            if (model.DirectorOrPartnerId.HasValue)
                Session.SetCurrentDopStatus(model.DirectorOrPartnerId.Value, model.IsDirector.IsDirector ?? false);

            Session.SetLoadedPage(id);
            return model;
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Part(int id, bool? back = false)
        {
            var model = SetupGetPart(id);

            return
                back.HasValue && back.Value
                    ? GetPreviousView(id, FormSection.PrincipalAuthority, model)
                    : GetNextView(id, FormSection.PrincipalAuthority, model);
        }


        private IActionResult PrincipalAuthorityPost<T>(T model, int submittedPageId, bool doDopLinking = true)
        {
            return PrincipalAuthorityPost(model, submittedPageId, doDopLinking, m => !ModelState.IsValid);
        }

        private IActionResult PrincipalAuthorityPost<T>(T model, int submittedPageId, bool doDopLinking, Func<T, bool> modelIsInvalid)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, submittedPageId);

            if (modelIsInvalid(model))
            {
                return View(GetViewPath(FormSection.PrincipalAuthority, submittedPageId), model);
            }

            if (Session.GetCurrentPaIsDirector() && doDopLinking)
            {
                LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.DirectorOrPartners, model,
                    Session.GetCurrentDopId());
            }

            var paId = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.PrincipalAuthorities, model,
                Session.GetCurrentPaId());

            Session.SetCurrentPaStatus(paId, doDopLinking);

            return CheckParentValidityAndRedirect(FormSection.PrincipalAuthority, submittedPageId);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveIsDirector(IsDirectorViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, 2);

            if (!ModelState.IsValid || !model.IsDirector.HasValue)
            {
                return View(GetViewPath(FormSection.PrincipalAuthority, 2), model);
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

                return RedirectToAction(FormSection.PrincipalAuthority, 4);
            }

            LicenceApplicationPostDataHandler.UnlinkDirectorOrPartnerFromPrincipalAuthority(Session.GetCurrentPaId());

            return CheckParentValidityAndRedirect(FormSection.PrincipalAuthority, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePrincipalAuthorityConfirmation(PrincipalAuthorityConfirmationViewModel model)
        {
            return PrincipalAuthorityPost(model, 3, false, m => !ModelState.IsValid || !m.WillProvideConfirmation);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveFullName(FullNameViewModel model)
        {
            return PrincipalAuthorityPost(model, 4);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAlternativeFullName(AlternativeFullNameViewModel model)
        {
            return PrincipalAuthorityPost(model, 5);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDateOfBirth(DateOfBirthViewModel model)
        {
            return PrincipalAuthorityPost(model, 6);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTownOfBirth(TownOfBirthViewModel model)
        {
            return PrincipalAuthorityPost(model, 7);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveCountryOfBirth(CountryOfBirthViewModel model)
        {
            return PrincipalAuthorityPost(model, 8);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveJobTitle(JobTitleViewModel model)
        {
            return PrincipalAuthorityPost(model, 9);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAddress(AddressViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, 10);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.PrincipalAuthority, 10), model);
            }

            if (Session.GetCurrentPaIsDirector())
            {
                LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(),
                    l => l.DirectorOrPartners.Single(dop => dop.Id == Session.GetCurrentDopId()), model);
            }

            LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(),
                x => x.PrincipalAuthorities.SingleOrDefault(pa => pa.Id == Session.GetCurrentPaId()), model);

            return CheckParentValidityAndRedirect(FormSection.PrincipalAuthority, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return PrincipalAuthorityPost(model, 11);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessExtension(BusinessExtensionViewModel model)
        {
            return PrincipalAuthorityPost(model, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePersonalMobileNumber(PersonalMobileNumberViewModel model)
        {
            return PrincipalAuthorityPost(model, 13);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePersonalEmailAddress(PersonalEmailAddressViewModel model)
        {
            return PrincipalAuthorityPost(model, 14);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNationalInsuranceNumber(NationalInsuranceNumberViewModel model)
        {
            return PrincipalAuthorityPost(model, 15);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNationality(NationalityViewModel model)
        {
            return PrincipalAuthorityPost(model, 16);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePassport(PassportViewModel model)
        {
            return PrincipalAuthorityPost(model, 17);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePrincipalAuthorityRightToWork(PrincipalAuthorityRightToWorkViewModel model)
        {
            return PrincipalAuthorityPost(model, 18);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUndischargedBankrupt(UndischargedBankruptViewModel model)
        {
            return PrincipalAuthorityPost(model, 19);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDisqualifiedDirector(DisqualifiedDirectorViewModel model)
        {
            return PrincipalAuthorityPost(model, 20);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRestraintOrders(RestraintOrdersViewModel model)
        {
            return PrincipalAuthorityPost(model, 21);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewPrincipalAuthorityRestraintOrders(RestraintOrdersViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, 22);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
            model = parent.RestraintOrdersViewModel;

            if ((model.HasRestraintOrders ?? false) && !model.RestraintOrders.Any())
            {
                ModelState.AddModelError(nameof(model.RestraintOrders), "Please enter details of the restraint or confiscation orders or civil recoveries that you have been the subject of.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.PrincipalAuthority, 22), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, 23);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUnspentConvictions(UnspentConvictionsViewModel model)
        {
            return PrincipalAuthorityPost(model, 23);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewPrincipalAuthorityUnspentConvictions(UnspentConvictionsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, 24);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
            model = parent.UnspentConvictionsViewModel;

            if ((model.HasUnspentConvictions ?? false) && !model.UnspentConvictions.Any())
            {
                ModelState.AddModelError(nameof(model.UnspentConvictions), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.PrincipalAuthority, 24), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, 25);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            return PrincipalAuthorityPost(model, 25);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewPrincipalAuthorityOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, 26);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
            model = parent.OffencesAwaitingTrialViewModel;

            if ((model.HasOffencesAwaitingTrial ?? false) && !model.OffencesAwaitingTrial.Any())
            {
                ModelState.AddModelError(nameof(model.OffencesAwaitingTrial), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.PrincipalAuthority, 26), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, 27);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviousLicence(PreviousLicenceViewModel model)
        {
            return PrincipalAuthorityPost(model, 27);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviousTradingNames(object model)
        {
            return PrincipalAuthorityPost(model, 28, false);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewPrincipalAuthorityPreviousTradingNames(object model)
        {
            Session.SetSubmittedPage(FormSection.PrincipalAuthority, 29);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<PrincipalAuthorityViewModel, PrincipalAuthority>(licenceId,
                    l => l.PrincipalAuthorities.Single(p => p.Id == Session.GetCurrentPaId()));
            //model = parent.PreviousTradingNames;

            //if ((model.HasPreviousTradingNames ?? false) && !model.PreviousTradingNames.Any())
            //{
            //    ModelState.AddModelError(nameof(model.PreviousTradingNames), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
            //    ViewData.Add("doOverride", true);
            //    return View(GetViewPath(FormSection.PrincipalAuthority, 29), model);
            //}

            return ValidateParentAndRedirect(parent, FormSection.PrincipalAuthority, 30);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviousExperience(PreviousExperienceViewModel model)
        {
            return PrincipalAuthorityPost(model, 30, false);
        }
    }
}