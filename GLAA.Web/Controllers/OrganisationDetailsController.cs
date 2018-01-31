using System.Collections.Generic;
using System.Linq;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class OrganisationDetailsController : LicenceApplicationBaseController
    {
        public OrganisationDetailsController(ISessionHelper session,
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
            Session.SetLoadedPage(id);
            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build<OrganisationDetailsViewModel>(licenceId);
            return GetNextView(id, FormSection.OrganisationDetails, model);
        }

        private IActionResult OrganisationDetailsPost<T>(T model, int submittedPageId)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, submittedPageId);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.OrganisationDetails, submittedPageId), model);
            }

            var licenceId = Session.GetCurrentLicenceId();

            if (model is AddressViewModel)
            {
                LicenceApplicationPostDataHandler.UpdateAddress(Session.GetCurrentLicenceId(), x => x, model as AddressViewModel);
            }
            else
            {
                LicenceApplicationPostDataHandler.Update(licenceId, x => x, model);
            }

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, submittedPageId);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOrganisationName(BusinessNameViewModel model)
        {
            // TODO Validation

            return OrganisationDetailsPost(model, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddPreviousTradingName(BusinessNameViewModel model)
        {
            model.PreviousTradingNames = model.PreviousTradingNames.Concat(new [] { new PreviousTradingNameViewModel() }).ToList();
            return View(GetViewPath(FormSection.OrganisationDetails, 2), model);
        }

        [HttpPost]
        [ExportModelState]
        [Route("Licence/Apply/OrganisationDetails/RemoveOrganisationName/{id}")]
        public IActionResult RemovePreviousTradingName(int id, BusinessNameViewModel model)
        {
            model.PreviousTradingNames.RemoveAt(id);
            return View(GetViewPath(FormSection.OrganisationDetails, 2), model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTradingName(object model)
        {
            return OrganisationDetailsPost(model, 3);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAddress(AddressViewModel model)
        {
            return OrganisationDetailsPost(model, 4);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return OrganisationDetailsPost(model, 5);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessMobileNumber(BusinessMobileNumberViewModel model)
        {
            return OrganisationDetailsPost(model, 6);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessEmailAddress(BusinessEmailAddressViewModel model)
        {
            return OrganisationDetailsPost(model, 7);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessWebsite(BusinessWebsiteViewModel model)
        {
            return OrganisationDetailsPost(model, 8);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            return OrganisationDetailsPost(model, 9);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveLegalStatus(LegalStatusViewModel model)
        {
            return OrganisationDetailsPost(model, 10);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePAYEERNStatus(PAYEERNStatusViewModel model)
        {
            return OrganisationDetailsPost(model, 11);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveVATStatus(VATStatusViewModel model)
        {
            return OrganisationDetailsPost(model, 12);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTaxReference(TaxReferenceViewModel model)
        {
            return OrganisationDetailsPost(model, 13);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOperatingIndustries(OperatingIndustriesViewModel model)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, 14);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.OrganisationDetails, 14), model);
            }

            var licenceId = Session.GetCurrentLicenceId();

            // TODO: This could be a mapping
            LicenceApplicationPostDataHandler.UpdateShellfishStatus(licenceId, model);

            LicenceApplicationPostDataHandler.Update(licenceId, x => x.OperatingIndustries,
                model.OperatingIndustries);

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, 14);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTurnover(TurnoverViewModel model)
        {
            return OrganisationDetailsPost(model, 15);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOperatingCountries(OperatingCountriesViewModel model)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, 16);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.OrganisationDetails, 16), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.OperatingCountries,
                model.OperatingCountries);

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, 16);
        }
    }
}