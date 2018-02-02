using System.Collections.Generic;
using System.Linq;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, FormSection.OrganisationDetails)
        {
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Part(int id, bool? back = false)
        {
            Session.SetLoadedPage(GetViewNameForController(id));
            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build<OrganisationDetailsViewModel>(licenceId);

            return back.HasValue && back.Value
                ? GetPreviousViewForController(id, model)
                : GetNextViewForController(id, model);
        }

        private IActionResult OrganisationDetailsPost<T>(T model, string submittedViewName)
        {
            SetSubmittedPageForController(submittedViewName);

            if (!ModelState.IsValid)
            {
                return View(submittedViewName, model);
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

            return CheckParentValidityAndRedirectForController(submittedViewName);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOrganisationName(BusinessNameViewModel model)
        {
            var viewName = "BusinessName";
            SetSubmittedPageForController(viewName);

            if (!ModelState.IsValid)
            {
                return View(viewName, model);
            }

            var licenceId = Session.GetCurrentLicenceId();
            
            LicenceApplicationPostDataHandler.Update(licenceId, x => x, model);
            LicenceApplicationPostDataHandler.UpdateAll(licenceId, x => x.PreviousTradingNames, model.PreviousTradingNames);

            return CheckParentValidityAndRedirectForController(viewName);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddPreviousTradingName(BusinessNameViewModel model)
        {
            model.PreviousTradingNames = model.PreviousTradingNames.Concat(new [] { new PreviousTradingNameViewModel() }).ToList();
            return View("BusinessName", model);
        }

        [HttpPost]
        [ExportModelState]
        [Route("Licence/Apply/OrganisationDetails/RemoveOrganisationName/{id}")]
        public IActionResult RemovePreviousTradingName(int id, BusinessNameViewModel model)
        {
            model.PreviousTradingNames.RemoveAt(id);
            return View("BusinessName", model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAddress(AddressViewModel model)
        {
            return OrganisationDetailsPost(model, "Address");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return OrganisationDetailsPost(model, "BusinessPhoneNumber");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessMobileNumber(BusinessMobileNumberViewModel model)
        {
            return OrganisationDetailsPost(model, "BusinessMobileNumber");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessEmailAddress(BusinessEmailAddressViewModel model)
        {
            return OrganisationDetailsPost(model, "BusinessEmailAddress");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveBusinessWebsite(BusinessWebsiteViewModel model)
        {
            return OrganisationDetailsPost(model, "BusinessWebsite");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveCommunicationPreference(CommunicationPreferenceViewModel model)
        {
            return OrganisationDetailsPost(model, "CommunicationPreference");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveLegalStatus(LegalStatusViewModel model)
        {
            return OrganisationDetailsPost(model, "LegalStatus");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SavePAYEERNStatus(PAYEERNStatusViewModel model)
        {
            return OrganisationDetailsPost(model, "PAYEERNStatus");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveVATStatus(VATStatusViewModel model)
        {
            return OrganisationDetailsPost(model, "VATStatus");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTaxReference(TaxReferenceViewModel model)
        {
            return OrganisationDetailsPost(model, "TaxReference");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOperatingIndustries(OperatingIndustriesViewModel model)
        {
            var viewName = "OperatingIndustries";

            SetSubmittedPageForController(viewName);

            if (!ModelState.IsValid)
            {
                return View(viewName, model);
            }

            var licenceId = Session.GetCurrentLicenceId();

            // TODO: This could be a mapping
            LicenceApplicationPostDataHandler.UpdateShellfishStatus(licenceId, model);

            LicenceApplicationPostDataHandler.Update(licenceId, x => x.OperatingIndustries,
                model.OperatingIndustries);

            return CheckParentValidityAndRedirectForController(viewName);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveTurnover(TurnoverViewModel model)
        {
            return OrganisationDetailsPost(model, "Turnover");
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveOperatingCountries(OperatingCountriesViewModel model)
        {
            var viewName = "OperatingCountries";
            SetSubmittedPageForController(viewName);

            if (!ModelState.IsValid)
            {
                return View(viewName, model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.OperatingCountries,
                model.OperatingCountries);

            return CheckParentValidityAndRedirectForController(viewName);
        }
    }
}