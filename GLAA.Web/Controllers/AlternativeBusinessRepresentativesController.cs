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
    public class AlternativeBusinessRepresentativesController : LicenceApplicationBaseController
    {
        public AlternativeBusinessRepresentativesController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler,
            ILicenceStatusViewModelBuilder licenceStatusViewModelBuilder,
            IFormDefinition formDefinition,
            IConstantService constantService, IReferenceDataProvider rdp) : base(session, licenceApplicationViewModelBuilder,
            licenceApplicationPostDataHandler, licenceStatusViewModelBuilder, formDefinition, constantService, rdp)
        {
        }

        private IActionResult AbrGet(string actionName, bool? back)
        {
            Session.ClearCurrentAbrId();

            var licenceId = Session.GetCurrentLicenceId();

            var model =
                LicenceApplicationViewModelBuilder
                    .Build<AlternativeBusinessRepresentativeCollectionViewModel>(licenceId);

            return back.HasValue && back.Value
                ? GetPreviousView(FormSection.AlternativeBusinessRepresentatives, actionName, model)
                : GetNextView(FormSection.AlternativeBusinessRepresentatives, actionName, model);
        }

        [HttpGet]
        [ExportModelState]
        public IActionResult Introduction(bool? back = false)
        {
            return AbrGet(nameof(Introduction), back);
        }

        [HttpGet]
        [ExportModelState]
        public IActionResult HasAlternativeBusinessRepresentatives(bool? back = false)
        {
            return AbrGet(nameof(HasAlternativeBusinessRepresentatives), back);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult HasAlternativeBusinessRepresentatives(AlternativeBusinessRepresentativeCollectionViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentatives, nameof(HasAlternativeBusinessRepresentatives));

            model = RepopulateDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(nameof(HasAlternativeBusinessRepresentatives), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction(FormSection.AlternativeBusinessRepresentatives, nameof(Summary));
        }

        [HttpGet]
        [ExportModelState]
        public IActionResult Summary(bool? back = false)
        {
            return AbrGet(nameof(Summary), back);
        }
    }
}