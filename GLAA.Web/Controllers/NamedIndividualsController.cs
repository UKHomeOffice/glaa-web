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
            IConstantService constantService, IReferenceDataProvider rdp) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, rdp)
        {
        }

        [HttpGet]
        [ExportModelState]
        public IActionResult Part(int id, bool? back = false)
    {
            var licenceId = Session.GetCurrentLicenceId();

            Session.ClearCurrentNamedIndividualId();

            var model = LicenceApplicationViewModelBuilder.Build<NamedIndividualCollectionViewModel>(licenceId);

        return back.HasValue && back.Value
        ? GetPreviousView(id, FormSection.NamedIndividuals, model)
        : GetNextView(id, FormSection.NamedIndividuals, model);
    }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveNamedIndividuals(NamedIndividualCollectionViewModel model)
        {
            Session.SetSubmittedPage(FormSection.NamedIndividuals, 2);

            model = RepopulateDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.NamedIndividuals, 2), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction(FormSection.NamedIndividuals, 3);
        }
    }
}