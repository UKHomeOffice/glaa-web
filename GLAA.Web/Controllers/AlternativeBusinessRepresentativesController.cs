using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class AlternativeBusinessRepresentativesController : LicenceApplicationBaseController
    {
        public AlternativeBusinessRepresentativesController(ISessionHelper session,
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
            Session.ClearCurrentAbrId();

            var licenceId = Session.GetCurrentLicenceId();

            var model =
                LicenceApplicationViewModelBuilder
                    .Build<AlternativeBusinessRepresentativeCollectionViewModel>(licenceId);

            return GetNextView(id, FormSection.AlternativeBusinessRepresentatives, model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAlternativeBusinessRepresentatives(AlternativeBusinessRepresentativeCollectionViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentatives, 2);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentatives, 2), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction(FormSection.AlternativeBusinessRepresentatives, 3);
        }
    }
}