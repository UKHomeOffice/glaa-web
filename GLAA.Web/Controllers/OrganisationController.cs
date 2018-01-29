using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class OrganisationController : LicenceApplicationBaseController
    {
        public OrganisationController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService)
        {
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Part(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build<OrganisationViewModel>(licenceId);
            return GetNextView(id, FormSection.Organisation, model);
        }

        private IActionResult OrganisationPost<T>(T model, int submittedPageId)
        {
            Session.SetSubmittedPage(FormSection.Organisation, submittedPageId);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Organisation, submittedPageId), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return CheckParentValidityAndRedirect(FormSection.Organisation, submittedPageId);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOutsideSectors(OutsideSectorsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.Organisation, 2);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Organisation, 2), model);
            }

            // update the non collection part
            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);
            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.SelectedSectors, model.SelectedSectors);

            return CheckParentValidityAndRedirect(FormSection.Organisation, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveWrittenAgreement(WrittenAgreementViewModel model)
        {
            return OrganisationPost(model, 3);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePscControlled(PSCControlledViewModel model)
        {
            return OrganisationPost(model, 4);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveMultipleBranch(MultipleBranchViewModel model)
        {
            Session.SetSubmittedPage(FormSection.Organisation, 5);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.Organisation, 5), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);
            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.SelectedMultiples, model.SelectedMultiples);

            return CheckParentValidityAndRedirect(FormSection.Organisation, 5);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTransportingWorkers(TransportingWorkersViewModel model)
        {
            return OrganisationPost(model, 6);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAccommodatingWorkers(AccommodatingWorkersViewModel model)
        {
            return OrganisationPost(model, 7);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveSourcingWorkers(SourcingWorkersViewModel model)
        {
            return OrganisationPost(model, 8);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveWorkerSupplyMethod(WorkerEmploymentStatusViewModel model)
        {
            return OrganisationPost(model, 9);
        }


        [HttpPost]
        [ExportModelState]
        public IActionResult SaveWorkerContract(WorkerContractViewModel model)
        {
            return OrganisationPost(model, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBannedFromTrading(BannedFromTradingViewModel model)
        {
            return OrganisationPost(model, 11);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveSubcontractor(SubcontractorViewModel model)
        {
            return OrganisationPost(model, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveShellfishWorkerNumber(ShellfishWorkerNumberViewModel model)
        {
            return OrganisationPost(model, 13);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveShellfishWorkerNationality(ShellfishWorkerNationalityViewModel model)
        {
            return OrganisationPost(model, 14);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePreviouslyWorkedInShellfish(PreviouslyWorkedInShellfishViewModel model)
        {
            return OrganisationPost(model, 15);
        }
    }
}