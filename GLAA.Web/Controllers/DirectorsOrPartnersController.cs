using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class DirectorsOrPartnersController : LicenceApplicationBaseController
    {
        public DirectorsOrPartnersController(ISessionHelper session,
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
            Session.ClearCurrentDopStatus();
            Session.ClearCurrentPaStatus();

            var licenceId = Session.GetCurrentLicenceId();

            var model = LicenceApplicationViewModelBuilder.Build<DirectorOrPartnerCollectionViewModel>(licenceId);

            return GetNextView(id, FormSection.DirectorsOrPartners, model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveDirectorsOrPartners(DirectorOrPartnerCollectionViewModel model)
        {
            Session.SetSubmittedPage(FormSection.DirectorsOrPartners, 2);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.DirectorsOrPartners, 2), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction(FormSection.DirectorsOrPartners, 3);
        }
    }
}