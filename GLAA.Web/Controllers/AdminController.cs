using System;
using GLAA.Services.Admin;
using GLAA.ViewModels;
using GLAA.ViewModels.Admin;
using GLAA.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace GLAA.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ISessionHelper session;
        private readonly IAdminHomeViewModelBuilder homeBuilder;
        private readonly IAdminLicenceListViewModelBuilder listBuilder;
        private readonly IAdminLicenceViewModelBuilder licenceBuilder;
        private readonly IAdminLicencePostDataHandler postDataHandler;
        private readonly IAdminUserListViewModelBuilder userListBuilder;
        private readonly IAdminUserViewModelBuilder userBuilder;
        private readonly IAdminUserPostDataHandler userPostDataHandler;

        public AdminController(ISessionHelper session, IAdminHomeViewModelBuilder homeBuilder,
            IAdminLicenceListViewModelBuilder listBuilder,
            IAdminLicenceViewModelBuilder licenceBuilder, IAdminLicencePostDataHandler postDataHandler,
            IAdminUserListViewModelBuilder userListBuilder, IAdminUserViewModelBuilder userBuilder, IAdminUserPostDataHandler updh)
        {
            this.session = session;
            this.homeBuilder = homeBuilder;
            this.listBuilder = listBuilder;
            this.licenceBuilder = licenceBuilder;
            this.postDataHandler = postDataHandler;
            this.userListBuilder = userListBuilder;
            this.userBuilder = userBuilder;
            this.userPostDataHandler = updh;
        }

        public ActionResult Index()
        {
            var model = homeBuilder.New();
            return View(model);
        }

        public ActionResult ApplicationList()
        {
            StringValues outputs;
            var qsVal = Request.Query.TryGetValue("isApplication", out outputs);
            var type = Convert.ToBoolean(outputs) ? LicenceOrApplication.Application : LicenceOrApplication.Licence;
            var model = listBuilder.Build(type);
            return View("ApplicationList", model);
        }

        [Route("Admin/Licence/{id}")]
        public ActionResult Licence(int id)
        {
            session.SetCurrentUserIsAdmin(true);
            session.SetCurrentLicenceId(id);
            var model = licenceBuilder.Build(id);
            return View("Application", model);
        }

        [HttpPost]
        [Route("Admin/Licence/{id}")]
        public ActionResult Licence(AdminLicenceViewModel model)
        {
            postDataHandler.UpdateStatus(model);

            model = licenceBuilder.Build(model.Licence.Id);

            return View("Application", model);
        }

        public ActionResult ApplicationPerson()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Users()
        {
            var model = userListBuilder.Build().GetAwaiter().GetResult();
            return View(model);
        }

        [HttpGet]
        [Route("Admin/EditUser/{id}")]
        public ActionResult EditUser(string id)
        {
            var model = userBuilder.Build(id);
            return View("UserDetails", model);
        }

        [HttpPost]
        public ActionResult EditUser(AdminUserViewModel model)
        {
            userPostDataHandler.Update(model);

            return RedirectToAction("Users", new {id = model.Id});
        }
    }
}