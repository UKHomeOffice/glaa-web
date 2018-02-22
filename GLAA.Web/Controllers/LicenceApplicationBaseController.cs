using System.Linq;
using GLAA.Domain.Models;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class LicenceApplicationBaseController : DefaultController
    {
        protected readonly ISessionHelper Session;
        protected readonly ILicenceApplicationViewModelBuilder LicenceApplicationViewModelBuilder;
        protected readonly ILicenceApplicationPostDataHandler LicenceApplicationPostDataHandler;
        protected readonly ILicenceStatusViewModelBuilder LicenceStatusViewModelBuilder;
        protected readonly IConstantService ConstantService;
        protected readonly IReferenceDataProvider ReferenceDataProvider;

        public LicenceApplicationBaseController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService, IReferenceDataProvider rdp) : base(formDefinition)
        {
            Session = session;
            LicenceApplicationViewModelBuilder = licenceApplicationViewModelBuilder;
            LicenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
            LicenceStatusViewModelBuilder = licenceStatusViewModelBuilder;
            ConstantService = constantService;
            ReferenceDataProvider = rdp;
        }

        private T RepopulateCountries<T>(T model) where T : INeedCountries
        {
            model.Countries = ReferenceDataProvider.GetCountries();
            return model;
        }

        private T RepopulateCounties<T>(T model) where T : INeedCounties
        {
            model.Counties = ReferenceDataProvider.GetCounties();
            return model;
        }

        protected T RepopulateDropdowns<T>(T model)
        {
            if (model is INeedCountries needsCountries)
            {
                model = (T)RepopulateCountries(needsCountries);
            }

            if (model is INeedCounties needsCounties)
            {
                model = (T)RepopulateCounties(needsCounties);
            }

            return model;
        }

        protected IActionResult CheckParentValidityAndRedirect(FormSection section, string actionName)
        {
            var licenceId = Session.GetCurrentLicenceId();

            if (!FormDefinition.IsNextPageLastPage(section, actionName))
            {
                var parent = FindParentSection(section, licenceId);

                return parent == null
                    ? RedirectToAction("TaskList", "Licence")
                    : ValidateParentAndRedirect(parent, section, actionName);
            }

            return RedirectToLastAction(section);
        }

        private IValidatable FindParentSection(FormSection section, int licenceId)
        {
            IValidatable parent;
            switch (section)
            {
                case FormSection.OrganisationDetails:
                    parent = LicenceApplicationViewModelBuilder.Build<OrganisationDetailsViewModel>(licenceId) ??
                             new OrganisationDetailsViewModel();
                    break;
                case FormSection.PrincipalAuthority:
                    parent = LicenceApplicationViewModelBuilder
                        .Build<PrincipalAuthorityViewModel, PrincipalAuthority>(
                            licenceId,
                            l => l.PrincipalAuthorities.SingleOrDefault(p => p.Id == Session.GetCurrentPaId()));
                    break;
                case FormSection.AlternativeBusinessRepresentative:
                    parent = LicenceApplicationViewModelBuilder
                        .Build<AlternativeBusinessRepresentativeViewModel, AlternativeBusinessRepresentative>(
                            licenceId,
                            l => l.AlternativeBusinessRepresentatives.SingleOrDefault(a =>
                                a.Id == Session.GetCurrentAbrId()));
                    break;
                case FormSection.DirectorOrPartner:
                    parent =
                        LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerViewModel, DirectorOrPartner>(
                            licenceId,
                            l => l.DirectorOrPartners.FirstOrDefault(d => d.Id == Session.GetCurrentDopId()));
                    break;
                case FormSection.NamedIndividual:
                    parent = LicenceApplicationViewModelBuilder.Build<NamedIndividualViewModel, NamedIndividual>(
                        licenceId,
                        l => l.NamedIndividuals.FirstOrDefault(n => n.Id == Session.GetCurrentNamedIndividualId()));
                    break;
                case FormSection.Organisation:
                    parent = LicenceApplicationViewModelBuilder.Build<OrganisationViewModel>(licenceId)
                             ?? new OrganisationViewModel();
                    break;
                default:
                    // Somehow we've saved a model without creating a parent
                    parent = null;
                    break;
            }
            return parent;
        }

        protected IActionResult ValidateParentAndRedirect(IValidatable parent, FormSection section, string actionName)
        {
            parent.Validate();

            if (parent.IsValid)
            {
                return RedirectToLastAction(section);
            }

            var nextAction = FormDefinition.GetNextPage(section, actionName).ActionName;
            return RedirectToAction(section, nextAction);
        }

        protected IActionResult CheckParentValidityAndRedirectBack(FormSection section, string actionName)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var previousAction = FormDefinition.GetPreviousPage(section, actionName);
            var parent = FindParentSection(section, licenceId);

            return parent == null
                ? RedirectBackToAction(section, previousAction.ActionName)
                : ValidateParentAndRedirectBack(parent, section, previousAction.ActionName);
        }

        protected IActionResult ValidateParentAndRedirectBack(IValidatable parent, FormSection section, string actionName)
        {
            parent.Validate();
            
            return parent.IsValid ? RedirectToLastAction(section) : RedirectBackToAction(section, actionName);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Back(FormSection section, string actionName, bool isSecurityPart = false)
        {
            return isSecurityPart
                ? RedirectBackToAction(section, actionName)
                : CheckParentValidityAndRedirectBack(section, actionName);
        }
    }
}