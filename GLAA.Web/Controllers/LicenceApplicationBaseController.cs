﻿using System;
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
        protected readonly IFormDefinition FormDefinition;
        protected readonly IConstantService ConstantService;

        protected readonly FormSection Section;

        public LicenceApplicationBaseController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService,
            FormSection section) : base(formDefinition)
        {
            Session = session;
            LicenceApplicationViewModelBuilder = licenceApplicationViewModelBuilder;
            LicenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
            LicenceStatusViewModelBuilder = licenceStatusViewModelBuilder;
            FormDefinition = formDefinition;
            ConstantService = constantService;
            Section = section;
        }

        protected IActionResult CheckParentValidityAndRedirectForController(string submittedViewName)
        {
            return CheckParentValidityAndRedirect(Section, submittedViewName);
        }

        private IActionResult CheckParentValidityAndRedirect(FormSection section, string submittedViewName)
        {
            var licenceId = Session.GetCurrentLicenceId();

            if (FormDefinition.NextViewIsFinalView(section, submittedViewName))
            {
                return RedirectToLastAction(section);
            }
            
            var parent = FindParentSection(section, licenceId);

            return parent == null
                ? RedirectToAction("TaskList", "Licence")
                : ValidateParentAndRedirect(parent, section, submittedViewName);
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

        protected IActionResult ValidateParentAndRedirect(IValidatable parent, FormSection section, string submittedViewName)
        {
            parent.Validate();

            return parent.IsValid
                ? RedirectToLastAction(section)
                : RedirectToAction(section, FormDefinition.GetNextViewNumber(section, submittedViewName));
        }

        protected IActionResult CheckParentValidityAndRedirectBack(FormSection section, int submittedPageId)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var nextPageId = submittedPageId - 1;
            var parent = FindParentSection(section, licenceId);

            return parent == null && nextPageId > 0
                ? RedirectBackToAction(section, nextPageId)
                : ValidateParentAndRedirectBack(parent, section, nextPageId);
        }

        protected IActionResult ValidateParentAndRedirectBack(IValidatable parent, FormSection section, int nextPageId)
        {
            parent.Validate();

            return nextPageId > 0
                ? (parent.IsValid ? RedirectToLastAction(section) : RedirectBackToAction(section, nextPageId))
                : RedirectToLastAction(section);
        }

        protected ActionResult GetNextViewForController<T>(int id, T model) where T : IValidatable
        {
            return GetNextView(id, Section, model);
        }

        protected ActionResult GetPreviousViewForController<T>(int id, T model) where T : IValidatable
        {
            return GetPreviousView(id, Section, model);
        }

        protected string GetViewNameForController(int id)
        {
            return GetViewName(Section, id);
        }

        protected void SetSubmittedPageForController(string viewName)
        {
            Session.SetSubmittedPage(Section, viewName);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Back(FormSection section, int submittedPageId, bool isSecurityPart = false)
        {
            return isSecurityPart
                ? RedirectBackToAction(section, submittedPageId)
                : CheckParentValidityAndRedirectBack(section, submittedPageId);
        }
    }
}