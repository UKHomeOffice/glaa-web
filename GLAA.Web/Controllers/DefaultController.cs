using GLAA.ViewModels;
using GLAA.Web.FormLogic;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class DefaultController : Controller
    {
        protected readonly IFormDefinition FormDefinition;

        public DefaultController(IFormDefinition formDefinition)
        {
            FormDefinition = formDefinition;
        }

        protected virtual string GetViewPath(FormSection section, int id)
        {
            return $"{section.ToString()}.{id}";
        }

        protected virtual string GetLastViewPath(FormSection section)
        {
            return GetViewPath(section, FormDefinition.GetSectionLength(section));
        }

        protected ActionResult GetPreviousView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, id, model))
                return RedirectToPreviousPossibleView(id, section, model);

            var viewPath = GetViewPath(section, id);
            var viewModel = FormDefinition.GetViewModel(section, id, model);

            return View(viewPath, viewModel);
        }

        protected ActionResult RedirectToPreviousPossibleView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            while (!FormDefinition.CanViewPage(section, id, model))
                id--;

            return RedirectBackToAction(section, id);
        }

        protected ActionResult GetNextView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, id, model))
                return RedirectToNextPossibleView(id, section, model);

            var viewPath = GetViewPath(section, id);
            var viewModel = FormDefinition.GetViewModel(section, id, model);

            return View(viewPath, viewModel);
        }

        protected ActionResult RedirectToNextPossibleView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            while (!FormDefinition.CanViewPage(section, id, model))
                id++;

            return RedirectToAction(section, id);
        }

        protected virtual ActionResult RedirectToAction(FormSection section, int id)
        {
            return RedirectToAction("Part", section.ToString(), new {id});
        }

        protected virtual ActionResult RedirectBackToAction(FormSection section, int id)
        {
            return RedirectToAction("Part", section.ToString(), new { id, back = true });
        }

        protected virtual ActionResult RedirectToLastAction(FormSection section)
        {
            return RedirectToAction("Part", section.ToString(), new {id = FormDefinition.GetSectionLength(section)});
        }
    }
}