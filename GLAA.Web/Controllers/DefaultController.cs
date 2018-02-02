using GLAA.ViewModels;
using GLAA.Web.FormLogic;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class DefaultController : Controller
    {
        private readonly IFormDefinition formDefinition;

        public DefaultController(IFormDefinition formDefinition)
        {
            this.formDefinition = formDefinition;
        }

        protected virtual string GetViewPath(FormSection section, int id)
        {
            return $"{section.ToString()}.{id}";
        }

        protected virtual string GetLastViewPath(FormSection section)
        {
            return GetViewPath(section, formDefinition.GetSectionLength(section));
        }

        protected string GetViewName(FormSection section, int id)
        {
            return formDefinition.GetViewName(section, id);
        }

        protected ActionResult GetPreviousView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            if (!formDefinition.CanViewNextModel(section, id, model))
                return RedirectToPreviousPossibleView(id, section, model);

            var viewPath = GetViewPath(section, id);
            var viewModel = formDefinition.GetViewModel(section, id, model);

            return View(viewPath, viewModel);
        }

        protected ActionResult RedirectToPreviousPossibleView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            while (!formDefinition.CanViewNextModel(section, id, model))
                id--;

            return RedirectBackToAction(section, id);
        }

        protected ActionResult GetNextView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            if (!formDefinition.CanViewNextModel(section, id, model))
                return RedirectToNextPossibleView(id, section, model);

            var viewPath = GetViewPath(section, id);
            var viewModel = formDefinition.GetViewModel(section, id, model);

            return View(viewPath, viewModel);
        }

        protected ActionResult RedirectToNextPossibleView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            while (!formDefinition.CanViewNextModel(section, id, model))
                id++;

            return RedirectToAction(section, id);
        }

        protected ActionResult RedirectToAction(FormSection section, int id)
        {
            return RedirectToAction("Part", section.ToString(), new {id});
        }

        protected ActionResult RedirectBackToAction(FormSection section, int id)
        {
            return RedirectToAction("Part", section.ToString(), new { id, back = true });
        }

        protected ActionResult RedirectToLastAction(FormSection section)
        {
            return RedirectToAction("Part", section.ToString(), new {id = formDefinition.GetSectionLength(section)});
        }
    }
}