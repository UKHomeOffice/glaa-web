using System.Linq;
using GLAA.Common;
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

        public LicenceApplicationBaseController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService, IReferenceDataProvider rdp) : base(formDefinition, rdp)
        {
            Session = session;
            LicenceApplicationViewModelBuilder = licenceApplicationViewModelBuilder;
            LicenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
            LicenceStatusViewModelBuilder = licenceStatusViewModelBuilder;
            ConstantService = constantService;
        }

        [System.Obsolete]
        protected IActionResult CheckParentValidityAndRedirect(FormSection section, int submittedPageId)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var sectionLength = FormDefinition.GetSectionLength(section);
            var nextPageId = submittedPageId + 1;

            if (nextPageId != sectionLength)
            {
                var parent = FindParentSection(section, licenceId);

                if (Session.GetCurrentUserIsAdmin())
                    return RedirectToAction("Licence", "Admin", new {id = licenceId});

                return parent == null
                    ? RedirectToAction("TaskList", "Licence")
                    : ValidateParentAndRedirect(parent, section, nextPageId);
            }

            return RedirectToLastAction(section);
        }

        protected IActionResult CheckParentValidityAndRedirect(FormSection section, string actionName)
        {
            var licenceId = Session.GetCurrentLicenceId();

            if (!FormDefinition.IsNextPageLastPage(section, actionName))
            {
                var parent = FindParentSection(section, licenceId);

                if (Session.GetCurrentUserIsAdmin())
                    return RedirectToAction("Licence", "Admin", new { id = licenceId });

                return parent == null
                    ? RedirectToAction("TaskList", "Licence")
                    : ValidateParentAndRedirect(parent, section, actionName);
            }

            return RedirectToLastActionForNewSection(section);
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

        [System.Obsolete]
        protected IActionResult ValidateParentAndRedirect(IValidatable parent, FormSection section, int nextPageId)
        {
            parent.Validate();

            return parent.IsValid ? RedirectToLastAction(section) : RedirectToAction(section, nextPageId);
        }

        protected IActionResult ValidateParentAndRedirect(IValidatable parent, FormSection section, string actionName)
        {
            parent.Validate();
            var nextActionName = FormDefinition.GetNextPage(section, actionName).ActionName;

            return parent.IsValid ? RedirectToLastActionForNewSection(section) : RedirectToAction(section, nextActionName);
        }

        [System.Obsolete]
        protected IActionResult CheckParentValidityAndRedirectBack(FormSection section, int submittedPageId, FormSection? parentSection = null)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var nextPageId = submittedPageId - 1;
            var parent = FindParentSection(section, licenceId);

            return parent == null && nextPageId > 0
                ? RedirectBackToAction(section, nextPageId)
                : ValidateParentAndRedirectBack(parent, section, nextPageId, parentSection);
        }

        protected IActionResult CheckParentValidityAndRedirectBack(FormSection section, string actionName, FormSection? parentSection = null)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var parent = FindParentSection(section, licenceId);

            if (parent != null)
            {
                return ValidateParentAndRedirectBack(parent, section, actionName, parentSection);
            }

            var previousPage = FormDefinition.GetPreviousPage(section, actionName);
            return RedirectBackToAction(section, previousPage.ActionName);
        }

        [System.Obsolete]
        protected IActionResult ValidateParentAndRedirectBack(IValidatable parent, FormSection section, int nextPageId, FormSection? parentSection = null)
        {
            parent.Validate();

            return nextPageId > 0
                ? (parent.IsValid ? RedirectToLastAction(parentSection ?? section) : RedirectBackToAction(section, nextPageId))
                : RedirectToLastAction(section);
        }

        protected IActionResult ValidateParentAndRedirectBack(IValidatable parent, FormSection section, string actionName, FormSection? parentSection = null)
        {
            parent.Validate();
            var prevPage = FormDefinition.GetPreviousPage(section, actionName);
            return parent.IsValid
                ? RedirectToLastActionForNewSection(parentSection ?? section)
                : RedirectBackToAction(section, prevPage.ActionName);
        }

        [HttpGet]
        [ImportModelState]
        [System.Obsolete]
        public IActionResult Back(FormSection section, int submittedPageId, bool isSecurityPart = false, FormSection? parentSection = null)
        {
            return isSecurityPart
                ? RedirectBackToAction(section, submittedPageId)
                : CheckParentValidityAndRedirectBack(section, submittedPageId, parentSection);
        }

        public IActionResult Back(FormSection section, string actionName, bool isSecurityPart = false, FormSection? parentSection = null)
        {
            return isSecurityPart
                ? RedirectBackToAction(section, actionName)
                : CheckParentValidityAndRedirectBack(section, actionName, parentSection);
        }
    }
}