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
        
        private IActionResult AlternativeBusinessRepresentativeGet(string actionName, bool? back)
        {
            var model = SetupGetPart(actionName);

            return back.HasValue && back.Value
                ? GetPreviousView(FormSection.AlternativeBusinessRepresentative, actionName, model)
                : GetNextView(FormSection.AlternativeBusinessRepresentative, actionName, model);
        }

        private AlternativeBusinessRepresentativeViewModel SetupGetPart(string actionName)
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

            Session.SetLoadedPage(actionName);
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
                return RedirectToLastActionForNewSection(FormSection.AlternativeBusinessRepresentatives);
            }

            Session.SetCurrentAbrId(id);

            var model = abrs.AlternativeBusinessRepresentatives.Single(a => a.Id == id);
            LicenceApplicationViewModelBuilder.BuildCountriesFor(model);

            return View(GetLastViewPathForNewSection(FormSection.AlternativeBusinessRepresentative), model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DeleteAlternativeBusinessRepresentative(AlternativeBusinessRepresentativeViewModel model)
        {
            var id = Session.GetCurrentAbrId();

            LicenceApplicationPostDataHandler.Delete<AlternativeBusinessRepresentative>(id);

            return RedirectToLastActionForNewSection(FormSection.AlternativeBusinessRepresentatives);
        }

        private IActionResult AlternativeBusinessRepresentativePost<T>(T model, string actionName)
        {
            return AlternativeBusinessRepresentativePost(model, actionName, m => !ModelState.IsValid);
        }

        private IActionResult AlternativeBusinessRepresentativePost<T>(T model, string actionName,
            Func<T, bool> modelIsInvalid)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, actionName);

            model = RepopulateDropdowns(model);

            if (modelIsInvalid(model))
            {
                return View(actionName, model);
            }

            var id = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.AlternativeBusinessRepresentatives,
                model, Session.GetCurrentAbrId());
            Session.SetCurrentAbrId(id);

            return CheckParentValidityAndRedirect(FormSection.AlternativeBusinessRepresentative, actionName);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Introduction(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(Introduction), back);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult FullName(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(FullName), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult FullName(FullNameViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(FullName));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult AlternativeName(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(AlternativeName), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AlternativeName(AlternativeFullNameViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(AlternativeName));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult DateOfBirth(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(DateOfBirth), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DateOfBirth(DateOfBirthViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(DateOfBirth));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BirthDetails(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(BirthDetails), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BirthDetails(BirthDetailsViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(BirthDetails));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult JobTitle(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(JobTitle), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult JobTitle(JobTitleViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(JobTitle));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Address(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(Address), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Address(AddressViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, nameof(Address));

            if (!ModelState.IsValid)
            {
                return View(nameof(Address), model);
            }

            LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(),
                x => x.AlternativeBusinessRepresentatives.Single(abr => abr.Id == Session.GetCurrentAbrId()), model);

            return CheckParentValidityAndRedirect(FormSection.AlternativeBusinessRepresentative, nameof(Address));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessPhoneNumber(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(BusinessPhoneNumber), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(BusinessPhoneNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessExtension(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(BusinessExtension), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessExtension(BusinessExtensionViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(BusinessExtension));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PersonalMobileNumber(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(PersonalMobileNumber), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PersonalMobileNumber(PersonalMobileNumberViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(PersonalMobileNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PersonalEmailAddress(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(PersonalEmailAddress), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PersonalEmailAddress(PersonalEmailAddressViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(PersonalEmailAddress));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Nationality(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(Nationality), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Nationality(NationalityViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(Nationality));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Passport(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(Passport), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Passport(PassportViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(Passport));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult RightToWork(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(RightToWork), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RightToWork(RightToWorkViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(RightToWork));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult UndischargedBankrupt(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(UndischargedBankrupt), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult UndischargedBankrupt(UndischargedBankruptViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(UndischargedBankrupt));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult DisqualifiedDirector(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(DisqualifiedDirector), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DisqualifiedDirector(DisqualifiedDirectorViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(DisqualifiedDirector));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult RestraintOrders(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(RestraintOrders), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RestraintOrders(RestraintOrdersViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(RestraintOrders));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult ReviewRestraintOrders(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(ReviewRestraintOrders), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewRestraintOrders(RestraintOrdersViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, nameof(ReviewRestraintOrders));

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                    l => l.AlternativeBusinessRepresentatives.Single(p => p.Id == Session.GetCurrentAbrId()));
            model = parent.RestraintOrders;

            if ((model.HasRestraintOrders ?? false) && !model.RestraintOrders.Any())
            {
                ModelState.AddModelError(nameof(model.RestraintOrders), "Please enter details of the restraint or confiscation orders or civil recoveries that you have been the subject of.");
                ViewData.Add("doOverride", true);
                return View(nameof(ReviewRestraintOrders), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.AlternativeBusinessRepresentative, nameof(ReviewRestraintOrders));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult UnspentConvictions(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(UnspentConvictions), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult UnspentConvictions(UnspentConvictionsViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(UnspentConvictions));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult ReviewUnspentConvictions(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(ReviewUnspentConvictions), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewUnspentConvictions(UnspentConvictionsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, nameof(ReviewUnspentConvictions));

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                    l => l.AlternativeBusinessRepresentatives.Single(p => p.Id == Session.GetCurrentAbrId()));
            model = parent.UnspentConvictions;

            if ((model.HasUnspentConvictions ?? false) && !model.UnspentConvictions.Any())
            {
                ModelState.AddModelError(nameof(model.UnspentConvictions), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(nameof(ReviewUnspentConvictions), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.AlternativeBusinessRepresentative, nameof(ReviewUnspentConvictions));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult OffencesAwaitingTrial(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(OffencesAwaitingTrial), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult OffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(OffencesAwaitingTrial));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult ReviewOffencesAwaitingTrial(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(ReviewOffencesAwaitingTrial), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentative, nameof(ReviewOffencesAwaitingTrial));

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(licenceId,
                    l => l.AlternativeBusinessRepresentatives.Single(p => p.Id == Session.GetCurrentAbrId()));
            model = parent.OffencesAwaitingTrial;

            if ((model.HasOffencesAwaitingTrial ?? false) && !model.OffencesAwaitingTrial.Any())
            {
                ModelState.AddModelError(nameof(model.OffencesAwaitingTrial), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(nameof(ReviewOffencesAwaitingTrial), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.AlternativeBusinessRepresentative, nameof(ReviewOffencesAwaitingTrial));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult PreviousLicence(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(PreviousLicence), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult PreviousLicence(PreviousLicenceViewModel model)
        {
            return AlternativeBusinessRepresentativePost(model, nameof(PreviousLicence));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Summary(bool back = false)
        {
            return AlternativeBusinessRepresentativeGet(nameof(Summary), back);
        }
    }
}