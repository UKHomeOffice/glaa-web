using System.Linq;
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
    public class JobTitleController : LicenceApplicationBaseController
    {
        public JobTitleController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, FormSection.JobTitle)
        {
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Part(int id, bool? back)
        {
            var licenceId = Session.GetCurrentLicenceId();
            var namedIndividualId = Session.GetCurrentNamedIndividualId();
            var model = LicenceApplicationViewModelBuilder
                .Build<NamedJobTitleViewModel, NamedJobTitle>(licenceId,
                    x => x.NamedJobTitles.FirstOrDefault(y => y.Id == namedIndividualId));

            return back.HasValue && back.Value
                ? GetPreviousView(id, FormSection.JobTitle, model)
                : GetNextView(id, FormSection.JobTitle, model);
        }

        [HttpGet]
        [ImportModelState]
        public IActionResult Review(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();

            var models =
                LicenceApplicationViewModelBuilder
                    .Build<NamedIndividualCollectionViewModel, NamedJobTitle>(licenceId,
                        x => x.NamedJobTitles);

            // TODO: A better defence against URL hacking?
            if (models.NamedJobTitles.All(ni => ni.Id != id))
            {
                return RedirectToAction(FormSection.NamedIndividuals, 3);
            }

            Session.SetCurrentNamedIndividualId(id);

            var model = models.NamedJobTitles.Single(a => a.Id == id);

            return View(GetLastViewPath(FormSection.JobTitle), model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveJobTitle(NamedJobTitleViewModel model)
        {
            Session.SetSubmittedPage(FormSection.JobTitle, 1);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.JobTitle, 1), model);
            }

            var id = LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x.NamedJobTitles, model, Session.GetCurrentNamedIndividualId());
            Session.SetCurrentNamedIndividualId(id);

            return RedirectToAction(FormSection.JobTitle, 2);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult DeleteNamedJobTitle(NamedJobTitleViewModel model)
        {
            var id = Session.GetCurrentNamedIndividualId();

            LicenceApplicationPostDataHandler.Delete<NamedJobTitle>(id);

            return RedirectToLastAction(FormSection.NamedIndividuals);
        }
    }
}