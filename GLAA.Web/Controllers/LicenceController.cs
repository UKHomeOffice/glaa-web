using System;
using GLAA.Domain.Models;
using GLAA.Web.Attributes;
using System.Linq;
using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GLAA.Web.Controllers
{
    //[SessionTimeout]
    public class LicenceController : LicenceApplicationBaseController
    {
        private readonly UserManager<GLAAUser> userManager;

        public LicenceController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService, IReferenceDataProvider rdp,
            UserManager<GLAAUser> userManager) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, rdp)
        {
            this.userManager = userManager;
        }

        [Route("Licence/TaskList")]
        public IActionResult TaskList()
        {
            Session.SetCurrentUserIsAdmin(false);
            Session.ClearCurrentPaStatus();
            Session.ClearCurrentAbrId();
            Session.ClearCurrentDopStatus();

            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build(licenceId);

            //TODO: It's feasible we could access this page with no licenceId where the model will be null
            //TODO: how should we handle this
            model.Declaration?.Validate();
            model.OrganisationDetails?.Validate();
            model.PrincipalAuthority?.Validate();
            model.AlternativeBusinessRepresentatives?.Validate();
            model.DirectorOrPartner?.Validate();
            model.NamedIndividuals?.Validate();
            model.Organisation?.Validate();

            return View(model);
        }

        #region Generic security questions
        // TODO: Use a custom route constraint to get the section as a FormSection
        // see http://www.c-sharpcorner.com/article/creating-custom-routing-constraint/
        // also https://github.com/aspnet/Routing/blob/dev/src/Microsoft.AspNetCore.Routing.Abstractions/IRouteConstraint.cs

        [HttpGet]
        [ImportModelState]
        public IActionResult AddRestraintOrder(FormSection section, int id)
        {
            Func<Licence, RestraintOrder> selector;

            switch (section)
            {
                case FormSection.PrincipalAuthority:
                    selector = l =>
                        l.PrincipalAuthorities.Single(pa => pa.Id == Session.GetCurrentPaId()).RestraintOrders
                            .SingleOrDefault(ro => ro.Id == id);
                    break;
                case FormSection.AlternativeBusinessRepresentative:
                    selector = l =>
                        l.AlternativeBusinessRepresentatives.Single(abr => abr.Id == Session.GetCurrentAbrId()).RestraintOrders
                            .SingleOrDefault(ro => ro.Id == id);
                    break;
                case FormSection.DirectorOrPartner:
                    selector = l =>
                        l.DirectorOrPartners.Single(dop => dop.Id == Session.GetCurrentDopId()).RestraintOrders
                            .SingleOrDefault(ro => ro.Id == id);
                    break;
                case FormSection.NamedIndividual:
                    selector = l =>
                        l.NamedIndividuals.Single(ni => ni.Id == Session.GetCurrentNamedIndividualId()).RestraintOrders
                            .SingleOrDefault(ro => ro.Id == id);
                    break;
                default:
                    selector = l => null;
                    break;
            }

            var model = LicenceApplicationViewModelBuilder.Build<RestraintOrderViewModel, RestraintOrder>(
                Session.GetCurrentLicenceId(), selector) ?? new RestraintOrderViewModel();

            return View("SecurityQuestions/RestraintOrder", model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddRestraintOrder(RestraintOrderViewModel model, FormSection section, int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddRestraintOrder", new { section, id });
            }

            switch (section)
            {
                case FormSection.PrincipalAuthority:
                {
                    var answerId = LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<RestraintOrderViewModel, RestraintOrder, PrincipalAuthority>(
                            Session.GetCurrentPaId(), id, model, pa => pa.RestraintOrders, ro => ro.PrincipalAuthority);

                    if (Session.GetCurrentPaIsDirector())
                    {
                        LicenceApplicationPostDataHandler
                            .UpsertSecurityAnswerAndLinkToParent<RestraintOrderViewModel, RestraintOrder, DirectorOrPartner>(
                                Session.GetCurrentDopId(), answerId, model, dop => dop.RestraintOrders,
                                ro => ro.DirectorOrPartner);
                    }
                    break;
                }
                case FormSection.AlternativeBusinessRepresentative:
                    LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<RestraintOrderViewModel, RestraintOrder,
                            AlternativeBusinessRepresentative>(
                            Session.GetCurrentAbrId(), id, model, abr => abr.RestraintOrders,
                            ro => ro.AlternativeBusinessRepresentative);
                    break;
                case FormSection.DirectorOrPartner:
                {
                    var answerId = LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<RestraintOrderViewModel, RestraintOrder, DirectorOrPartner>(
                            Session.GetCurrentDopId(), id, model, dop => dop.RestraintOrders, ro => ro.DirectorOrPartner);

                    if (Session.GetCurrentDopIsPa())
                    {
                        LicenceApplicationPostDataHandler
                            .UpsertSecurityAnswerAndLinkToParent<RestraintOrderViewModel, RestraintOrder, PrincipalAuthority>(
                                Session.GetCurrentPaId(), answerId, model, pa => pa.RestraintOrders, ro => ro.PrincipalAuthority);
                    }
                    break;
                }
                case FormSection.NamedIndividual:
                    LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<RestraintOrderViewModel, RestraintOrder,
                            NamedIndividual>(
                            Session.GetCurrentNamedIndividualId(), id, model, ni => ni.RestraintOrders,
                            ro => ro.NamedIndividual);
                    break;
            }

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RemoveRestraintOrder(RestraintOrderViewModel model, FormSection section, int id)
        {
            LicenceApplicationPostDataHandler.Delete<RestraintOrder>(id);

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult AddConviction(FormSection section, int id)
        {
            Func<Licence, Conviction> selector;

            switch (section)
            {
                case FormSection.PrincipalAuthority:
                    selector = l =>
                        l.PrincipalAuthorities.Single(pa => pa.Id == Session.GetCurrentPaId()).UnspentConvictions
                            .SingleOrDefault(uc => uc.Id == id);
                    break;
                case FormSection.AlternativeBusinessRepresentative:
                    selector = l =>
                        l.AlternativeBusinessRepresentatives.Single(abr => abr.Id == Session.GetCurrentAbrId()).UnspentConvictions
                            .SingleOrDefault(uc => uc.Id == id);
                    break;
                case FormSection.DirectorOrPartner:
                    selector = l =>
                        l.DirectorOrPartners.Single(dop => dop.Id == Session.GetCurrentDopId()).UnspentConvictions
                            .SingleOrDefault(uc => uc.Id == id);
                    break;
                case FormSection.NamedIndividual:
                    selector = l =>
                        l.NamedIndividuals.Single(ni => ni.Id == Session.GetCurrentNamedIndividualId()).UnspentConvictions
                            .SingleOrDefault(uc => uc.Id == id);
                    break;
                default:
                    selector = l => null;
                    break;
            }

            var model =
                LicenceApplicationViewModelBuilder.Build<UnspentConvictionViewModel, Conviction>(
                    Session.GetCurrentLicenceId(), selector) ?? new UnspentConvictionViewModel();

            return View("SecurityQuestions/UnspentConviction", model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddConviction(UnspentConvictionViewModel model, FormSection section, int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddConviction", new { section, id });
            }

            switch (section)
            {
                case FormSection.PrincipalAuthority:
                {
                    var answerId = LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<UnspentConvictionViewModel, Conviction, PrincipalAuthority>(
                            Session.GetCurrentPaId(), id, model, pa => pa.UnspentConvictions, uc => uc.PrincipalAuthority);

                    if (Session.GetCurrentPaIsDirector())
                    {
                        LicenceApplicationPostDataHandler
                            .UpsertSecurityAnswerAndLinkToParent<UnspentConvictionViewModel, Conviction, DirectorOrPartner>(
                                Session.GetCurrentDopId(), answerId, model, dop => dop.UnspentConvictions,
                                uc => uc.DirectorOrPartner);
                    }
                    break;
                }
                case FormSection.AlternativeBusinessRepresentative:
                    LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<UnspentConvictionViewModel, Conviction,
                            AlternativeBusinessRepresentative>(
                            Session.GetCurrentAbrId(), id, model, abr => abr.UnspentConvictions,
                            uc => uc.AlternativeBusinessRepresentative);
                    break;
                case FormSection.DirectorOrPartner:
                {
                    var answerId = LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<UnspentConvictionViewModel, Conviction, DirectorOrPartner>(
                            Session.GetCurrentDopId(), id, model, dop => dop.UnspentConvictions, uc => uc.DirectorOrPartner);

                    if (Session.GetCurrentDopIsPa())
                    {
                        LicenceApplicationPostDataHandler
                            .UpsertSecurityAnswerAndLinkToParent<UnspentConvictionViewModel, Conviction, PrincipalAuthority>(
                                Session.GetCurrentPaId(), answerId, model, pa => pa.UnspentConvictions,
                                uc => uc.PrincipalAuthority);
                    }
                    break;
                }
                case FormSection.NamedIndividual:
                    LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<UnspentConvictionViewModel, Conviction,
                            NamedIndividual>(
                            Session.GetCurrentNamedIndividualId(), id, model, ni => ni.UnspentConvictions,
                            uc => uc.NamedIndividual);
                    break;
            }

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RemoveConviction(UnspentConvictionViewModel model, FormSection section, int id)
        {
            LicenceApplicationPostDataHandler.Delete<Conviction>(id);

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult AddOffenceAwaitingTrial(FormSection section, int id)
        {
            Func<Licence, OffenceAwaitingTrial> selector;

            switch (section)
            {
                case FormSection.PrincipalAuthority:
                    selector = l =>
                        l.PrincipalAuthorities.Single(pa => pa.Id == Session.GetCurrentPaId()).OffencesAwaitingTrial
                            .SingleOrDefault(o => o.Id == id);
                    break;
                case FormSection.AlternativeBusinessRepresentative:
                    selector = l =>
                        l.AlternativeBusinessRepresentatives.Single(abr => abr.Id == Session.GetCurrentAbrId()).OffencesAwaitingTrial
                            .SingleOrDefault(o => o.Id == id);
                    break;
                case FormSection.DirectorOrPartner:
                    selector = l =>
                        l.DirectorOrPartners.Single(dop => dop.Id == Session.GetCurrentDopId()).OffencesAwaitingTrial
                            .SingleOrDefault(o => o.Id == id);
                    break;
                case FormSection.NamedIndividual:
                    selector = l =>
                        l.NamedIndividuals.Single(ni => ni.Id == Session.GetCurrentNamedIndividualId()).OffencesAwaitingTrial
                            .SingleOrDefault(o => o.Id == id);
                    break;
                default:
                    selector = l => null;
                    break;
            }

            var model = LicenceApplicationViewModelBuilder.Build<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial>(
                            Session.GetCurrentLicenceId(), selector) ?? new OffenceAwaitingTrialViewModel();

            return View("SecurityQuestions/OffenceAwaitingTrial", model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddOffenceAwaitingTrial(OffenceAwaitingTrialViewModel model, FormSection section, int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddOffenceAwaitingTrial", new { section, id });
            }

            switch (section)
            {
                case FormSection.PrincipalAuthority:
                {
                    var answerId = LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial,
                            PrincipalAuthority>(
                            Session.GetCurrentPaId(), id, model, pa => pa.OffencesAwaitingTrial,
                            o => o.PrincipalAuthority);

                    if (Session.GetCurrentPaIsDirector())
                    {
                        LicenceApplicationPostDataHandler
                            .UpsertSecurityAnswerAndLinkToParent<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial,
                                DirectorOrPartner>(
                                Session.GetCurrentDopId(), answerId, model, dop => dop.OffencesAwaitingTrial,
                                o => o.DirectorOrPartner);
                    }
                    break;
                }
                case FormSection.AlternativeBusinessRepresentative:
                    LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial,
                            AlternativeBusinessRepresentative>(
                            Session.GetCurrentAbrId(), id, model, abr => abr.OffencesAwaitingTrial,
                            o => o.AlternativeBusinessRepresentative);
                    break;
                case FormSection.DirectorOrPartner:
                {
                    var answerId = LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial,
                            DirectorOrPartner>(
                            Session.GetCurrentDopId(), id, model, dop => dop.OffencesAwaitingTrial,
                            o => o.DirectorOrPartner);

                    if (Session.GetCurrentDopIsPa())
                    {
                        LicenceApplicationPostDataHandler
                            .UpsertSecurityAnswerAndLinkToParent<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial,
                                PrincipalAuthority>(
                                Session.GetCurrentPaId(), answerId, model, pa => pa.OffencesAwaitingTrial,
                                o => o.PrincipalAuthority);
                    }
                    break;
                }
                case FormSection.NamedIndividual:
                    LicenceApplicationPostDataHandler
                        .UpsertSecurityAnswerAndLinkToParent<OffenceAwaitingTrialViewModel, OffenceAwaitingTrial,
                            NamedIndividual>(
                            Session.GetCurrentNamedIndividualId(), id, model, ni => ni.OffencesAwaitingTrial,
                            o => o.NamedIndividual);
                    break;
            }

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RemoveOffenceAwaitingTrial(OffenceAwaitingTrialViewModel model, FormSection section, int id)
        {
            LicenceApplicationPostDataHandler.Delete<OffenceAwaitingTrial>(id);

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult AddPreviousTradingName(string section, int id)
        {
            var model = LicenceApplicationViewModelBuilder.Build<PreviousTradingNameViewModel, PreviousTradingName>(
                            Session.GetCurrentLicenceId(),
                            l => l.PreviousTradingNames.SingleOrDefault(p => p.Id == id)) ?? new PreviousTradingNameViewModel();

            return View("SecurityQuestions/PreviousTradingName", model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult AddPreviousTradingName(PreviousTradingNameViewModel model, FormSection section, int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddPreviousTradingName", new { section, id });
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), l => l.PreviousTradingNames, model, model.Id);

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult RemovePreviousTradingName(PreviousTradingNameViewModel model, string section, int id)
        {
            LicenceApplicationPostDataHandler.Delete<PreviousTradingName>(id);

            var lastLoaded = Session.GetLoadedPage();

            return RedirectToAction(section, lastLoaded);
        }

        #endregion

        [Route("Licence/Apply/Fees/Part/{id}")]
        public IActionResult Fees(int id)
        {
            return View($"Fees.{id}");
        }

        [Route("Licence/Apply/Summary/Part/{id}")]
        public IActionResult Summary(int id)
        {
            return View($"Summary.{id}");
        }

        [Route("Licence/SubmitApplication")]
        [HttpGet]
        [ImportModelState]
        public IActionResult SubmitApplication()
        {
            var licenceId = Session.GetCurrentLicenceId();
            var model = LicenceApplicationViewModelBuilder.Build(licenceId);

            LicenceApplicationViewModelBuilder.BuildCountriesFor(model.PrincipalAuthority);

            model.DirectorOrPartner.DirectorsOrPartners =
                model.DirectorOrPartner.DirectorsOrPartners.Select(LicenceApplicationViewModelBuilder
                    .BuildCountriesFor);
            model.AlternativeBusinessRepresentatives.AlternativeBusinessRepresentatives =
                model.AlternativeBusinessRepresentatives.AlternativeBusinessRepresentatives.Select(
                    LicenceApplicationViewModelBuilder.BuildCountriesFor);

            model.Validate();

            return View(model);
        }

        [HttpPost]
        [ExportModelState]
        [Route("Licence/SubmitApplication")]
        public IActionResult SubmitApplication(LicenceApplicationViewModel model)
        {
            var licenceId = Session.GetCurrentLicenceId();

            if (!model.AgreedToTermsAndConditions)
            {
                ModelState.AddModelError("AgreedToTermsAndConditions", "You must agree to the terms and conditions in order to submit your application.");

                var dbModel = LicenceApplicationViewModelBuilder.Build(licenceId);
                model.OrganisationDetails = dbModel.OrganisationDetails;
                model.PrincipalAuthority = dbModel.PrincipalAuthority;
                model.AlternativeBusinessRepresentatives = dbModel.AlternativeBusinessRepresentatives;
                model.DirectorOrPartner = dbModel.DirectorOrPartner;
                model.NamedIndividuals = dbModel.NamedIndividuals;
                model.Organisation = dbModel.Organisation;
                return View("SubmitApplication", model);
            }
            
            model.NewLicenceStatus = new LicenceStatusViewModel
            {
                Id = ConstantService.ApplicationSubmittedOnlineStatusId
            };

            LicenceApplicationPostDataHandler.Update(licenceId, model);

            return RedirectToAction("Portal");
        }

        [Route("Licence/Portal")]
        [HttpGet]
        public IActionResult Portal()
        {
            var licenceId = Session.GetCurrentLicenceId();

            var model = LicenceApplicationViewModelBuilder.Build(licenceId);

            ViewData["IsSubmitted"] = false;

            model.NewLicenceStatus = LicenceStatusViewModelBuilder.BuildLatestStatus(licenceId);
            
            if (model.NewLicenceStatus.Id == ConstantService.ApplicationSubmittedOnlineStatusId 
                || model.NewLicenceStatus.Id == ConstantService.ApplicationSubmittedByPhoneId)
            {
                ViewData["IsSubmitted"] = true;
            } 

            return View(nameof(Portal), model);

        }

        [Authorize]
        [Route("Licence/ViewApplication")]
        [HttpGet]
        public async Task<IActionResult> ViewApplication()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            var model = LicenceApplicationViewModelBuilder.BuildLicencesForUser(user.Id).FirstOrDefault();

            var licenceId = Session.GetCurrentLicenceId();

            if (model != null)
            {
                model.NewLicenceStatus = LicenceStatusViewModelBuilder.BuildLatestStatus(model.Id);

                ViewData["IsSubmitted"] = model.NewLicenceStatus.Id == ConstantService.ApplicationSubmittedOnlineStatusId;

                return View(model);
            }
            else
            {
                return RedirectToAction(nameof(TaskList));
            }
        }

        [HttpGet]
        [Route("Licence/Resume")]
        public IActionResult Resume()
        {
            var model = new ResumeApplicationViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Licence/Resume")]
        public IActionResult Resume(ResumeApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {
                //TODO: We need a smaller / quicker check for existance here
                var licence = LicenceApplicationViewModelBuilder.Build(model.ApplicationId);

                if (licence != null)
                {
                    Session.SetCurrentLicenceId(licence.Id);
                    Session.SetString("ApplicationId", model.ApplicationId);

                    return RedirectToAction("TaskList");
                }

                ModelState.AddModelError("ApplicationNotFound",
                    $"We were unable to find your application with the ID: {model.ApplicationId}.");
                ViewData.Add("doOverride", true);
            }

            return View(model);
        }
    }
}