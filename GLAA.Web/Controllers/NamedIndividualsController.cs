using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class NamedIndividualsController : LicenceApplicationBaseController
    {
        public NamedIndividualsController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService)
        {
        }

        [HttpGet]
        [ExportModelState]
        public IActionResult Part(int id)
        {
            var licenceId = Session.GetCurrentLicenceId();

            Session.ClearCurrentNamedIndividualId();

            var model = LicenceApplicationViewModelBuilder.Build<NamedIndividualCollectionViewModel>(licenceId);

            return GetNextView(id, FormSection.NamedIndividuals, model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNamedIndividuals(NamedIndividualCollectionViewModel model)
        {
            Session.SetSubmittedPage(FormSection.NamedIndividuals, 2);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.NamedIndividuals, 2), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction(FormSection.NamedIndividuals, 3);
        }
    }
}