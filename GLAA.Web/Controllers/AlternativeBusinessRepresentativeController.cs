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
    public class AlternativeBusinessRepresentativeController : LicenceApplicationBaseController
    {
        public AlternativeBusinessRepresentativeController(ISessionHelper session,
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
        public IActionResult Part(int id, bool? back)
        {
            var model = SetupGetPart(id);

            return back.HasValue && back.Value
                ? GetPreviousView(id, FormSection.AlternativeBusinessRepresentative, model)
                : GetNextView(id, FormSection.AlternativeBusinessRepresentative, model);
        }

        private AlternativeBusinessRepresentativeViewModel SetupGetPart(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var abrId = Session.GetCurrentAbrId();

            var model =
                LicenceApplicationViewModelBuilder
                    .Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                        x => x.AlternativeBusinessRepresentatives.FirstOrDefault(a => a.Id == abrId)) ??
                new AlternativeBusinessRepresentativeViewModel();

            if (model.Id.HasValue)
            {
                Session.SetCurrentAbrId(model.Id.Value);
            }

            if (ViewData["IsSubmitted"] == null)
            {
                var currentStatus = LicenceStatusViewModelBuilder.BuildLatestStatus(licenceId);
                ViewData["IsSubmitted"] = currentStatus.Id == ConstantService.ApplicationSubmittedOnlineStatusId;
            }

            Session.SetLoadedPage(id);
            return model;
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Review(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();

            var abrs =
                LicenceApplicationViewModelBuilder
                    .Build<AlternativeBusinessRepresentativeCollectionViewModel, AlternativeBusinessRepresentative>(licenceId,
                        x => x.AlternativeBusinessRepresentatives);

            // TODO: A better defence against URL hacking?
            if (abrs.AlternativeBusinessRepresentatives.None(a => a.Id == id))
            {
                return RedirectToAction(FormSection.AlternativeBusinessRepresentatives, 3);
            }

            Session.SetCurrentAbrId(id);

            var model = abrs.AlternativeBusinessRepresentatives.Single(a => a.Id == id);
            LicenceApplicationViewModelBuilder.BuildCountriesFor(model);

            return View(GetLastViewPath(FormSection.AlternativeBusinessRepresentative), model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DeleteAlternativeBusinessRepresentative(AlternativeBusinessRepresentativeViewModel model)
        {
            var id = Session.GetCurrentAbrId();

            LicenceApplicationPostDataHandler.Delete<AlternativeBusinessRepresentative>(id);

            return RedirectToLastAction(FormSection.AlternativeBusinessRepresentatives);
        }

        private IActionResult AlternativeBusinessRepresentativePost<T>(T model, int submittedPageId)
        {
            return AlternativeBusinessRepresentativePost(model, submittedPageId, m => !ModelState.IsValid);
        }

        private IActionResult AlternativeBusinessRepresentativePost<T>(T model, int submittedPageId,
            Func<T, bool> modelIsInvalid)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, submittedPageId);

            model = RepopulateDropdowns(model);

            if (modelIsInvalid(model))
            {
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentative, submittedPageId), model);
            }

            var id = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.AlternativeBusinessRepresentatives,
                model, Session.GetCurrentAbrId());
            Session.SetCurrentAbrId(id);

            return CheckParentValidityAndRedirect(FormSection.AlternativeBusinessRepresentative, submittedPageId);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveFullName(FullNameViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 1);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAlternativeFullName(AlternativeFullNameViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDateOfBirth(DateOfBirthViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 3);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTownOfBirth(BirthDetailsViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 4);
        }

        //[HttpPost]
        //[ExportModelState]
        //public IActionResult SaveCountryOfBirth(CountryOfBirthViewModel model)
        //{
        //    return AlternativeBusinessRepresentativePost(model, 5);
        //}

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveJobTitle(JobTitleViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 5);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAddress(AddressViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, 6);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentative, 6), model);
            }

            LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(),
                x => x.AlternativeBusinessRepresentatives.Single(abr => abr.Id == Session.GetCurrentAbrId()), model);

            return CheckParentValidityAndRedirect(FormSection.AlternativeBusinessRepresentative, 6);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 7);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessExtension(BusinessExtensionViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 8);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePersonalMobileNumber(PersonalMobileNumberViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 9);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePersonalEmailAddress(PersonalEmailAddressViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNationality(NationalityViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 11);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePassport(PassportViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRightToWork(RightToWorkViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 13);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUndischargedBankrupt(UndischargedBankruptViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 14);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDisqualifiedDirector(DisqualifiedDirectorViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 15);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRestraintOrders(RestraintOrdersViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 16);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewAlternativeBusinessRepresentativeRestraintOrders(RestraintOrdersViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, 17);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                    l => l.AlternativeBusinessRepresentatives.Single(p => p.Id == Session.GetCurrentAbrId()));
            model = parent.RestraintOrdersViewModel;

            if ((model.HasRestraintOrders ?? false) && !model.RestraintOrders.Any())
            {
                ModelState.AddModelError(nameof(model.RestraintOrders), "Please enter details of the restraint or confiscation orders or civil recoveries that you have been the subject of.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentative, 17), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.AlternativeBusinessRepresentative, 18);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUnspentConvictions(UnspentConvictionsViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 18);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewAlternativeBusinessRepresentativeUnspentConvictions(UnspentConvictionsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, 19);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                    l => l.AlternativeBusinessRepresentatives.Single(p => p.Id == Session.GetCurrentAbrId()));
            model = parent.UnspentConvictionsViewModel;

            if ((model.HasUnspentConvictions ?? false) && !model.UnspentConvictions.Any())
            {
                ModelState.AddModelError(nameof(model.UnspentConvictions), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentative, 19), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.AlternativeBusinessRepresentative, 20);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 20);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewAlternativeBusinessRepresentativeOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, 21);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                    l => l.AlternativeBusinessRepresentatives.Single(p => p.Id == Session.GetCurrentAbrId()));
            model = parent.OffencesAwaitingTrialViewModel;

            if ((model.HasOffencesAwaitingTrial ?? false) && !model.OffencesAwaitingTrial.Any())
            {
                ModelState.AddModelError(nameof(model.OffencesAwaitingTrial), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentative, 21), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.AlternativeBusinessRepresentative, 22);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviousLicence(PreviousLicenceViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, 22);
        }
    }
}