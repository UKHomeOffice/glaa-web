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
    public class DirectorOrPartnerController : LicenceApplicationBaseController
    {
        public DirectorOrPartnerController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService, IReferenceDataProvider rdp) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, rdp)
        {
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Review(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();

            var dops = LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerCollectionViewModel>(licenceId);

            // TODO: A better defence against URL hacking?
            if (dops.DirectorsOrPartners.None(a => a.Id == id))
            {
                return RedirectToAction(FormSection.DirectorsOrPartners, 2);
            }

            var model = dops.DirectorsOrPartners.Single(a => a.Id == id);
            LicenceApplicationViewModelBuilder.BuildCountriesFor(model);

            Session.SetCurrentDopStatus(id, model.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority ?? false);

            if ((model.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority ?? false) && model.PrincipalAuthorityId.HasValue)
            {
                Session.SetCurrentPaStatus(model.PrincipalAuthorityId.Value, true);
            }

            return View(GetLastViewPath(FormSection.DirectorOrPartner), model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DeleteDirectorOrPartner(DirectorOrPartnerViewModel model)
        {
            LicenceApplicationPostDataHandler.Delete<DirectorOrPartner>(Session.GetCurrentDopId());

            if (Session.GetCurrentDopIsPa())
            {
                LicenceApplicationPostDataHandler.Delete<PrincipalAuthority>(Session.GetCurrentPaId());
            }

            return RedirectToLastAction(FormSection.DirectorsOrPartners);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Part(int id, bool? back)
        {
            var model = SetupGetPart(id);

            return back.HasValue && back.Value
                ? GetPreviousView(id, FormSection.DirectorOrPartner, model)
                : GetNextView(id, FormSection.DirectorOrPartner, model);
        }

        private DirectorOrPartnerViewModel SetupGetPart(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var dopId = Session.GetCurrentDopId();

            var model = LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerViewModel, DirectorOrPartner>(
                licenceId, l => l.DirectorOrPartners.SingleOrDefault(d => d.Id == dopId));

            if (model.Id.HasValue)
            {
                Session.SetCurrentDopStatus(model.Id.Value,
                    model.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority ?? false);
            }

            if (model.PrincipalAuthorityId.HasValue)
            {
                Session.SetCurrentPaStatus(model.PrincipalAuthorityId.Value,
                    model.IsPreviousPrincipalAuthority.IsPreviousPrincipalAuthority ?? false);
            }

            if (ViewData["IsSubmitted"] == null)
            {
                var currentStatus = LicenceStatusViewModelBuilder.BuildLatestStatus(licenceId);
                ViewData["IsSubmitted"] = currentStatus.Id == ConstantService.ApplicationSubmittedOnlineStatusId;
            }

            Session.SetLoadedPage(id);
            return model;
        }

        private IActionResult DirectorOrPartnerPost<T>(T model, int submittedPageId, bool doPaLinking = true)
        {
            return DirectorOrPartnerPost(model, submittedPageId, doPaLinking, m => !ModelState.IsValid);
        }

        private IActionResult DirectorOrPartnerPost<T>(T model, int submittedPageId, bool doPaLinking, Func<T, bool> modelIsInvalid)
        {
            Session.SetSubmittedPage(FormSection.DirectorOrPartner, submittedPageId);

            model = RepopulateDropdowns(model);

            if (modelIsInvalid(model))
            {
                return View(GetViewPath(FormSection.DirectorOrPartner, submittedPageId), model);
            }

            if (Session.GetCurrentDopIsPa() && doPaLinking)
            {
                LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.PrincipalAuthorities, model,
                    Session.GetCurrentPaId());
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.DirectorOrPartners, model,
                Session.GetCurrentDopId());

            return CheckParentValidityAndRedirect(FormSection.DirectorOrPartner, submittedPageId);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveIsPreviousPrincipalAuthority(IsPreviousPrincipalAuthorityViewModel model)
        {
            Session.SetSubmittedPage(FormSection.DirectorOrPartner, 1);

            if (!ModelState.IsValid || !model.IsPreviousPrincipalAuthority.HasValue)
            {
                return View(GetViewPath(FormSection.DirectorOrPartner, 1), model);
            }

            var dopId = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.DirectorOrPartners, model,
                Session.GetCurrentDopId());

            Session.SetCurrentDopStatus(dopId, model.IsPreviousPrincipalAuthority.Value);

            if (model.IsPreviousPrincipalAuthority.Value)
            {
                var paId = LicenceApplicationPostDataHandler.UpsertPrincipalAuthorityAndLinkToDirectorOrPartner(
                    Session.GetCurrentLicenceId(), dopId, Session.GetCurrentPaId(), model);
                Session.SetCurrentPaStatus(paId, true);
            }
            else
            {
                LicenceApplicationPostDataHandler.UnlinkPrincipalAuthorityFromDirectorOrPartner(Session.GetCurrentDopId());
            }

            return CheckParentValidityAndRedirect(FormSection.DirectorOrPartner, 1);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveFullName(FullNameViewModel model)
        {
            return DirectorOrPartnerPost(model, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAlternativeFullName(AlternativeFullNameViewModel model)
        {
            return DirectorOrPartnerPost(model, 3);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDateOfBirth(DateOfBirthViewModel model)
        {
            return DirectorOrPartnerPost(model, 4);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTownOfBirth(TownOfBirthViewModel model)
        {
            return DirectorOrPartnerPost(model, 5);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveCountryOfBirth(CountryOfBirthViewModel model)
        {
            return DirectorOrPartnerPost(model, 6);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveJobTitle(JobTitleViewModel model)
        {
            return DirectorOrPartnerPost(model, 7);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAddress(AddressViewModel model)
        {
            Session.SetSubmittedPage(FormSection.DirectorOrPartner, 8);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.DirectorOrPartner, 8), model);
            }

            if (Session.GetCurrentDopIsPa())
            {
                LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(), l => l.PrincipalAuthorities.Single(pa => pa.Id == Session.GetCurrentPaId()), model);
            }

            LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(), l => l.DirectorOrPartners.Single(dop => dop.Id == Session.GetCurrentDopId()), model);

            return CheckParentValidityAndRedirect(FormSection.DirectorOrPartner, 8);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return DirectorOrPartnerPost(model, 9);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessExtension(BusinessExtensionViewModel model)
        {
            return DirectorOrPartnerPost(model, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePersonalMobileNumber(PersonalMobileNumberViewModel model)
        {
            return DirectorOrPartnerPost(model, 11);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePersonalEmailAddress(PersonalEmailAddressViewModel model)
        {
            return DirectorOrPartnerPost(model, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNationalInsuranceNumber(NationalInsuranceNumberViewModel model)
        {
            return DirectorOrPartnerPost(model, 13);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNationality(NationalityViewModel model)
        {
            return DirectorOrPartnerPost(model, 14);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePassport(PassportViewModel model)
        {
            return DirectorOrPartnerPost(model, 15);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRightToWork(RightToWorkViewModel model)
        {
            return DirectorOrPartnerPost(model, 16);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUndischargedBankrupt(UndischargedBankruptViewModel model)
        {
            return DirectorOrPartnerPost(model, 17);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDisqualifiedDirector(DisqualifiedDirectorViewModel model)
        {
            return DirectorOrPartnerPost(model, 18);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRestraintOrders(RestraintOrdersViewModel model)
        {
            return DirectorOrPartnerPost(model, 19);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewDirectorOrPartnerRestraintOrders(RestraintOrdersViewModel model)
        {
            Session.SetSubmittedPage(FormSection.DirectorOrPartner, 20);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerViewModel, DirectorOrPartner>(licenceId,
                    l => l.DirectorOrPartners.Single(p => p.Id == Session.GetCurrentDopId()));
            model = parent.RestraintOrders;

            if ((model.HasRestraintOrders ?? false) && !model.RestraintOrders.Any())
            {
                ModelState.AddModelError(nameof(model.RestraintOrders), "Please enter details of the restraint or confiscation orders or civil recoveries that you have been the subject of.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.DirectorOrPartner, 20), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.DirectorOrPartner, 21);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUnspentConvictions(UnspentConvictionsViewModel model)
        {
            return DirectorOrPartnerPost(model, 21);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewDirectorOrPartnerUnspentConvictions(UnspentConvictionsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.DirectorOrPartner, 22);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerViewModel, DirectorOrPartner>(licenceId,
                    l => l.DirectorOrPartners.Single(p => p.Id == Session.GetCurrentDopId()));
            model = parent.UnspentConvictions;

            if ((model.HasUnspentConvictions ?? false) && !model.UnspentConvictions.Any())
            {
                ModelState.AddModelError(nameof(model.UnspentConvictions), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.DirectorOrPartner, 22), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.DirectorOrPartner, 23);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            return DirectorOrPartnerPost(model, 23);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewDirectorOrPartnerOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            Session.SetSubmittedPage(FormSection.DirectorOrPartner, 24);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerViewModel, DirectorOrPartner>(licenceId,
                    l => l.DirectorOrPartners.Single(p => p.Id == Session.GetCurrentDopId()));
            model = parent.OffencesAwaitingTrial;

            if ((model.HasOffencesAwaitingTrial ?? false) && !model.OffencesAwaitingTrial.Any())
            {
                ModelState.AddModelError(nameof(model.OffencesAwaitingTrial), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.DirectorOrPartner, 24), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.DirectorOrPartner, 24);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviousLicence(PreviousLicenceViewModel model)
        {
            return DirectorOrPartnerPost(model, 25);
        }
    }
}