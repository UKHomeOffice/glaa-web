﻿using GLAA.Common;
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

        [HttpGet]
        [ExportModelState]
        public IActionResult Part(int id, bool? back)
        {
            Session.ClearCurrentAbrId();

            var licenceId = Session.GetCurrentLicenceId();

            var model =
                LicenceApplicationViewModelBuilder
                    .Build<AlternativeBusinessRepresentativeCollectionViewModel>(licenceId);

            return back.HasValue && back.Value
                ? GetPreviousView(id, FormSection.AlternativeBusinessRepresentatives, model)
                : GetNextView(id, FormSection.AlternativeBusinessRepresentatives, model);
        }

        [HttpPost]
        [ExportModelState]
        public IActionResult SaveAlternativeBusinessRepresentatives(AlternativeBusinessRepresentativeCollectionViewModel model)
        {
            Session.SetSubmittedPage(FormSection.AlternativeBusinessRepresentatives, 2);

            model = RepopulateDropdowns(model);

            if (!ModelState.IsValid)
            {
                return View(GetViewPath(FormSection.AlternativeBusinessRepresentatives, 2), model);
            }

            LicenceApplicationPostDataHandler.Update(Session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction(FormSection.AlternativeBusinessRepresentatives, 3);
        }
    }
}