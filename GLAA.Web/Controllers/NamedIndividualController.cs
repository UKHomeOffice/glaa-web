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
    public class NamedIndividualController : LicenceApplicationBaseController
    {
        public NamedIndividualController(ISessionHelper session,
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
                ? GetPreviousView(id, FormSection.NamedIndividual, model)
                : GetNextView(id, FormSection.NamedIndividual, model);
        }

        private NamedIndividualViewModel SetupGetPart(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var namedIndividualId = Session.GetCurrentNamedIndividualId();

            var model = LicenceApplicationViewModelBuilder
                .Build<NamedIndividualViewModel, NamedIndividual>(licenceId,
                    x => x.NamedIndividuals.FirstOrDefault(y => y.Id == namedIndividualId));

            if (model.Id.HasValue)
                Session.SetCurrentNamedIndividualId(model.Id.Value);

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

            var models =
                LicenceApplicationViewModelBuilder
                    .Build<NamedIndividualCollectionViewModel, NamedIndividual>(licenceId,
                        x => x.NamedIndividuals);

            // TODO: A better defence against URL hacking?
            if (models.NamedIndividuals.All(ni => ni.Id != id))
                return RedirectToAction(FormSection.NamedIndividuals, 3);

            Session.SetCurrentNamedIndividualId(id);

            var model = models.NamedIndividuals.Single(a => a.Id == id);

            return View(GetLastViewPath(FormSection.NamedIndividual), model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DeleteNamedIndividual(NamedIndividualViewModel model)
        {
            var id = Session.GetCurrentNamedIndividualId();

            LicenceApplicationPostDataHandler.Delete<NamedIndividual>(id);

            return RedirectToLastAction(FormSection.NamedIndividuals);
        }

        private IActionResult NamedIndividualPost<T>(T model, int submittedPageId)
        {
            return NamedIndividualPost(model, submittedPageId, m => !ModelState.IsValid);
        }

        private IActionResult NamedIndividualPost<T>(T model, int submittedPageId, Func<T, bool> modelIsInvalid)
        {
            Session.SetSubmittedPage(FormSection.NamedIndividual, submittedPageId);

            model = RepopulateDropdowns(model);

            if (modelIsInvalid(model))
            {
                return View(GetViewPath(FormSection.NamedIndividual, submittedPageId), model);
            }

            var id = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.NamedIndividuals, model, Session.GetCurrentNamedIndividualId());
            Session.SetCurrentNamedIndividualId(id);

            return CheckParentValidityAndRedirect(FormSection.NamedIndividual, submittedPageId);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveFullName(FullNameViewModel model)
        {
            return NamedIndividualPost(model, 1);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDateOfBirth(DateOfBirthViewModel model)
        {
            return NamedIndividualPost(model, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return NamedIndividualPost(model, 3);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessExtension(BusinessExtensionViewModel model)
        {
            return NamedIndividualPost(model, 4);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRightToWork(RightToWorkViewModel model)
        {
            return NamedIndividualPost(model, 5);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUndischargedBankrupt(UndischargedBankruptViewModel model)
        {
            return NamedIndividualPost(model, 6);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDisqualifiedDirector(DisqualifiedDirectorViewModel model)
        {
            return NamedIndividualPost(model, 7);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveRestraintOrders(RestraintOrdersViewModel model)
        {
            return NamedIndividualPost(model, 8);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewNamedIndividualRestraintOrders(RestraintOrdersViewModel model)
        {
            Session.SetSubmittedPage(FormSection.NamedIndividual, 9);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<NamedIndividualViewModel, NamedIndividual>(licenceId,
                    l => l.NamedIndividuals.Single(p => p.Id == Session.GetCurrentNamedIndividualId()));
            model = parent.RestraintOrdersViewModel;

            if ((model.HasRestraintOrders ?? false) && !model.RestraintOrders.Any())
            {
                ModelState.AddModelError(nameof(model.RestraintOrders), "Please enter details of the restraint or confiscation orders or civil recoveries that you have been the subject of.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.NamedIndividual, 9), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.NamedIndividual, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveUnspentConvictions(UnspentConvictionsViewModel model)
        {
            return NamedIndividualPost(model, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewNamedIndividualUnspentConvictions(UnspentConvictionsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.NamedIndividual, 11);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<NamedIndividualViewModel, NamedIndividual>(licenceId,
                    l => l.NamedIndividuals.Single(p => p.Id == Session.GetCurrentNamedIndividualId()));
            model = parent.UnspentConvictionsViewModel;

            if ((model.HasUnspentConvictions ?? false) && !model.UnspentConvictions.Any())
            {
                ModelState.AddModelError(nameof(model.UnspentConvictions), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.NamedIndividual, 11), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.NamedIndividual, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            return NamedIndividualPost(model, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult ReviewNamedIndividualOffencesAwaitingTrial(OffencesAwaitingTrialViewModel model)
        {
            Session.SetSubmittedPage(FormSection.NamedIndividual, 13);

            var licenceId = Session.GetCurrentLicenceId();
            var parent =
                LicenceApplicationViewModelBuilder.Build<NamedIndividualViewModel, NamedIndividual>(licenceId,
                    l => l.NamedIndividuals.Single(p => p.Id == Session.GetCurrentNamedIndividualId()));
            model = parent.OffencesAwaitingTrialViewModel;

            if ((model.HasOffencesAwaitingTrial ?? false) && !model.OffencesAwaitingTrial.Any())
            {
                ModelState.AddModelError(nameof(model.OffencesAwaitingTrial), "Please enter details of the unspent criminal convictions, or alternative sanctions or penalties for proven offences you have.");
                ViewData.Add("doOverride", true);
                return View(GetViewPath(FormSection.NamedIndividual, 13), model);
            }

            return ValidateParentAndRedirect(parent, FormSection.NamedIndividual, 14);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviousLicence(PreviousLicenceViewModel model)
        {
            return NamedIndividualPost(model, 14);
        }
    }
}