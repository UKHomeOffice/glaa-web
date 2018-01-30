using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GLAA.Domain.Models;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels;
using GLAA.ViewModels.LicenceApplication;
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
        protected readonly IFormDefinition FormDefinition;
        protected readonly IConstantService ConstantService;

        public LicenceApplicationBaseController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService) : base(formDefinition)
        {
            Session = session;
            LicenceApplicationViewModelBuilder = licenceApplicationViewModelBuilder;
            LicenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
            LicenceStatusViewModelBuilder = licenceStatusViewModelBuilder;
            FormDefinition = formDefinition;
            ConstantService = constantService;
        }

        protected IActionResult CheckParentValidityAndRedirect(FormSection section, int submittedPageId)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var sectionLength = FormDefinition.GetSectionLength(section);
            var nextPageId = submittedPageId + 1;

            if (nextPageId != sectionLength)
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
                        return RedirectToAction("TaskList", "Licence");
                }

                return ValidateParentAndRedirect(parent, section, nextPageId);
            }

            return RedirectToLastAction(section);
        }

        protected IActionResult ValidateParentAndRedirect(IValidatable parent, FormSection section, int nextPageId)
        {
            parent.Validate();

            if (parent.IsValid)
            {
                return RedirectToLastAction(section);
            }

            return RedirectToAction(section, nextPageId);
        }
    }
}