using GLAA.Services;
using GLAA.ViewModels;
using GLAA.Web.FormLogic;
using Microsoft.AspNetCore.Mvc;

namespace GLAA.Web.Controllers
{
    public class DefaultController : Controller
    {
        protected readonly IFormDefinition FormDefinition;
        protected readonly IReferenceDataProvider ReferenceDataProvider;

        public DefaultController(IFormDefinition formDefinition, IReferenceDataProvider rdp)
        {
            FormDefinition = formDefinition;
            ReferenceDataProvider = rdp;
        }

        private T RepopulateCountries<T>(T model) where T : INeedCountries
        {
            model.Countries = ReferenceDataProvider.GetCountries();
            return model;
        }

        private T RepopulateCounties<T>(T model) where T : INeedCounties
        {
            model.Counties = ReferenceDataProvider.GetCounties();
            return model;
        }

        protected T RepopulateDropdowns<T>(T model)
        {
            if (model is INeedCountries needsCountries)
            {
                model = (T)RepopulateCountries(needsCountries);
            }

            if (model is INeedCounties needsCounties)
            {
                model = (T)RepopulateCounties(needsCounties);
            }

            return model;
        }

        protected virtual string GetViewPath(FormSection section, string actionName)
        {
            return actionName;
        }

        protected virtual string GetLastViewPath(FormSection section)
        {
            return GetViewPath(section, FormDefinition.GetLastPage(section).ActionName);
        }

        protected ActionResult GetPreviousView<T>(FormSection section, string actionName, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, actionName, model))
            {
                return RedirectToPreviousPossibleView(section, actionName, model);
            }
            
            var viewModel = FormDefinition.GetViewModel(section, actionName, model);

            return View(actionName, viewModel);
        }

        protected ActionResult RedirectToPreviousPossibleView<T>(FormSection section, string actionName, T model) where T : IValidatable
        {
            var action = FormDefinition.GetPreviousPossibleAction(section, actionName, model);

            return RedirectBackToAction(section, action.ActionName);
        }

        protected ActionResult GetNextView<T>(FormSection section, string actionName, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, actionName, model))
            {
                return RedirectToNextPossibleView(section, actionName, model);
            }
            
            var viewModel = FormDefinition.GetViewModel(section, actionName, model);

            return View(actionName, viewModel);
        }

        protected ActionResult RedirectToNextPossibleView<T>(FormSection section, string actionName, T model) where T : IValidatable
        {
            var action = FormDefinition.GetNextPossibleAction(section, actionName, model);
            
            return RedirectToAction(section, action.ActionName);
        }

        protected virtual ActionResult RedirectToAction(FormSection section, string actionName)
        {
            return RedirectToAction(actionName, section.ToString());
        }

        protected virtual ActionResult RedirectBackToAction(FormSection section, string actionName)
        {
            return RedirectToAction(actionName, section.ToString(), new { back = true });
        }

        protected virtual ActionResult RedirectToLastAction(FormSection section)
        {
            var last = FormDefinition.GetLastPage(section);

            return RedirectToAction(last.ActionName, section.ToString());
        }
    }
}