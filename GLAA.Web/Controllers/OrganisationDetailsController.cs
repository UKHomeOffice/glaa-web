using System.Linq;
using GLAA.Common;
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
            IConstantService constantService, IReferenceDataProvider rdp) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, rdp)
        {
        }
        
        private IActionResult OrganisationDetailsGet(string actionName, bool? back = false)
        {
            Session.SetLoadedPage(actionName);

            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build<OrganisationDetailsViewModel>(licenceId);

            return back.HasValue && back.Value
                ? GetPreviousView(FormSection.OrganisationDetails, actionName, model)
                : GetNextView(FormSection.OrganisationDetails, actionName, model);
        }

        private IActionResult OrganisationDetailsPost<T>(T model, string actionName)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, actionName);

            model = RepopulateDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(actionName, model);
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

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, actionName);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Introduction(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(Introduction), back);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessName(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(BusinessName), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessName(BusinessNameViewModel model)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, nameof(BusinessName));

            model.Validate();

            if (!model.IsValid)
            {
                return View(nameof(BusinessName), model);
            }

            var licenceId = Session.GetCurrentLicenceId();
            
            LicenceApplicationPostDataHandler.Update(licenceId, x => x, model);
            LicenceApplicationPostDataHandler.UpdateAll(licenceId, x => x.PreviousTradingNames, model.PreviousTradingNames);

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, nameof(BusinessName));
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddPreviousTradingName(BusinessNameViewModel model)
        {
            model.PreviousTradingNames = model.PreviousTradingNames.Concat(new [] { new PreviousTradingNameViewModel() }).ToList();
            return View(nameof(BusinessName), model);
        }

        [HttpPost]
        [ExportModelState]
        [Route("Licence/Apply/OrganisationDetails/RemoveOrganisationName/{id}")]
        public IActionResult RemovePreviousTradingName(int id, BusinessNameViewModel model)
        {
            model.PreviousTradingNames.RemoveAt(id);
            return View(nameof(BusinessName), model);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Address(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(Address), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Address(AddressViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(Address));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessPhoneNumber(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(BusinessPhoneNumber), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessPhoneNumber(BusinessPhoneNumberViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(BusinessPhoneNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessMobileNumber(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(BusinessMobileNumber), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessMobileNumber(BusinessMobileNumberViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(BusinessMobileNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessEmailAddress(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(BusinessEmailAddress), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessEmailAddress(BusinessEmailAddressViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(BusinessMobileNumber));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessWebsite(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(BusinessWebsite), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessWebsite(BusinessWebsiteViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(BusinessWebsite));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult CommunicationPreference(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(CommunicationPreference), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult CommunicationPreference(CommunicationPreferenceViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(CommunicationPreference));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult LegalStatus(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(LegalStatus), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult LegalStatus(LegalStatusViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(LegalStatus));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult BusinessCredentials(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(BusinessCredentials), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult BusinessCredentials(BusinessCredentialsViewModel model)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, nameof(BusinessCredentials));

            if (!ModelState.IsValid)
            {
                return View(nameof(BusinessCredentials), model);
            }

            var licenceId = Session.GetCurrentLicenceId();

            LicenceApplicationPostDataHandler.Update(licenceId, x => x, model);
            LicenceApplicationPostDataHandler.UpdateAll(licenceId, x => x.PAYENumbers, model.PAYEStatusViewModel.PAYENumbers);

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, nameof(BusinessCredentials));
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddPAYENumber(BusinessCredentialsViewModel model)
        {
            model.Validate();

            model.PAYEStatusViewModel.PAYENumbers = model.PAYEStatusViewModel.PAYENumbers.Concat(new[] { new PAYENumberRow() }).ToList();
            return View(nameof(BusinessCredentials), model);
        }

        [HttpPost]
        [ExportModelState]
        [Route("Licence/Apply/OrganisationDetails/RemovePAYENumber/{id}")]
        public IActionResult RemovePAYENumber(int id, BusinessCredentialsViewModel model)
        {
            model.PAYEStatusViewModel.PAYENumbers.RemoveAt(id);
            return View(nameof(BusinessCredentials), model);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult VATStatus(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(VATStatus), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult VATStatus(VATStatusViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(VATStatus));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult TaxReference(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(TaxReference), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult TaxReference(TaxReferenceViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(TaxReference));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult OperatingIndustries(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(OperatingIndustries), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult OperatingIndustries(OperatingIndustriesViewModel model)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, nameof(OperatingIndustries));

            if (!ModelState.IsValid)
            {
                return View(nameof(OperatingIndustries), model);
            }

            var licenceId = Session.GetCurrentLicenceId();

            // TODO: This could be a mapping
            LicenceApplicationPostDataHandler.UpdateShellfishStatus(licenceId, model);

            LicenceApplicationPostDataHandler.Update(licenceId, x => x.OperatingIndustries,
                model.OperatingIndustries);

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, nameof(OperatingIndustries));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Turnover(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(Turnover), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult Turnover(TurnoverViewModel model)
        {
            return OrganisationDetailsPost(model, nameof(Turnover));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult OperatingCountries(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(OperatingCountries), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult OperatingCountries(OperatingCountriesViewModel model)
        {
            Session.SetSubmittedPage(FormSection.OrganisationDetails, nameof(OperatingCountries));

            if (!ModelState.IsValid)
            {
                return View(nameof(OperatingCountries), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.OperatingCountries,
                model.OperatingCountries);

            return CheckParentValidityAndRedirect(FormSection.OrganisationDetails, nameof(OperatingCountries));
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Summary(bool? back = false)
        {
            return OrganisationDetailsGet(nameof(Summary), back);
        }
    }
}