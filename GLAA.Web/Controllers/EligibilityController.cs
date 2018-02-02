using GLAA.Services;
using GLAA.Services.LicenceApplication;
using GLAA.ViewModels.LicenceApplication;
using GLAA.Web.Attributes;
using GLAA.Web.FormLogic;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class EligibilityController : DefaultController
    {
        private readonly ISessionHelper session;
        private readonly ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder;
        private readonly ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler;
        private readonly IConstantService constantService;

        public EligibilityController(ISessionHelper session,
            ILicenceApplicationViewModelBuilder licenceApplicationViewModelBuilder, IFormDefinition formDefinition,
            ILicenceApplicationPostDataHandler licenceApplicationPostDataHandler, IConstantService constantService)
            : base(formDefinition)
        {
            this.session = session;
            this.licenceApplicationViewModelBuilder = licenceApplicationViewModelBuilder;
            this.licenceApplicationPostDataHandler = licenceApplicationPostDataHandler;
            this.constantService = constantService;
        }

        protected override string GetViewPath(FormSection section, int id)
        {
            return $"{section.ToString()}.{id}";
        }

        [HttpGet]
        [ImportModelState]
        [Route("Eligibility/Part/{id}")]
        public ActionResult Eligibility(int id)
        {
            var licenceId = session.GetCurrentLicenceId();

            var model = licenceApplicationViewModelBuilder.Build<EligibilityViewModel>(licenceId);

            return GetNextView(id, FormSection.Eligibility, model);            
        }

        [HttpGet]
        public ActionResult Introduction()
        {
            var licenceApplicationModel = licenceApplicationViewModelBuilder.New();

            return View("Introduction", licenceApplicationModel);
        }

        [HttpPost]
        [ExportModelState]
        public ActionResult Introduction(LicenceApplicationViewModel model)
        {
            model.NewLicenceStatus = new LicenceStatusViewModel
            {
                Id = constantService.NewApplicationStatusId
            };

            var licenceId = licenceApplicationPostDataHandler.Insert(model);

            session.SetCurrentLicenceId(licenceId);

            return RedirectToAction("Part1");
        }

        [HttpPost]
        [ExportModelState]
        [Route("Eligibility/Part/1")]
        public ActionResult Part1(SuppliesWorkersViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part1");
            session.SetInt("LastSubmittedPageId", 1);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.1", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("Part2");            
        }

        [HttpPost]
        [ExportModelState]
        [Route("Eligibility/Part/2")]
        public ActionResult Part2(OperatingIndustriesViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part2");
            session.SetInt("LastSubmittedPageId", 2);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.2", model);
            }

            var licenceId = session.GetCurrentLicenceId();
            
            licenceApplicationPostDataHandler.UpdateShellfishStatus(licenceId, model);

            licenceApplicationPostDataHandler.Update(licenceId, x => x.OperatingIndustries,
                model.OperatingIndustries);

            return RedirectToAction("Part3");
        }

        [HttpPost]
        [ExportModelState]
        [Route("Eligibility/Part/3")]
        public ActionResult Part3(TurnoverViewModel model)
        {
            session.SetString("LastSubmittedPageSection", "Part3");
            session.SetInt("LastSubmittedPageId", 3);

            if (!ModelState.IsValid)
            {
                return View("Eligibility.3", model);
            }

            licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("Part4");
        }

        [HttpPost]
        [ExportModelState]
        [Route("Eligibility/Part/4")]
        public ActionResult Part4()
        {
            session.SetString("LastSubmittedPageSection", "Part4");
            session.SetInt("LastSubmittedPageId", 4);


            //if (!ModelState.IsValid)
            //{
            //    return View("Eligibility.3", model);
            //}

            //licenceApplicationPostDataHandler.Update(session.GetCurrentLicenceId(), x => x, model);

            return RedirectToAction("TaskList", "Licence");
        }
    }
}