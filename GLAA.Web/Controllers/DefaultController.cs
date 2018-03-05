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

        [System.Obsolete]
        protected virtual string GetViewPath(FormSection section, int id)
        {
            return $"{section.ToString()}.{id}";
        }

        [System.Obsolete]
        protected virtual string GetLastViewPath(FormSection section)
        {
            return GetViewPath(section, FormDefinition.GetSectionLength(section));
        }

        protected virtual string GetLastViewPathForNewSection(FormSection section)
        {
            return FormDefinition.GetLastPage(section).ActionName;
        }

        [System.Obsolete]
        protected ActionResult GetPreviousView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, id, model))
                return RedirectToPreviousPossibleView(id, section, model);

            var viewPath = GetViewPath(section, id);
            var viewModel = FormDefinition.GetViewModel(section, id, model);

            return View(viewPath, viewModel);
        }

        /// <summary>
        /// Return a view for the requested action name. If this view can't be viewed, it will seek backwards 
        /// through the pages until one that can be viewed is found. If no view can be found then the last view 
        /// will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the parent view model.</typeparam>
        /// <param name="section">The form section.</param>
        /// <param name="actionName">The name of the action to get the view for.</param>
        /// <param name="model">The parent view model.</param>
        /// <returns>A <see cref="ViewResult"/> for the current view or a <see cref="RedirectToActionResult"/> for the next possible view.</returns>
        protected IActionResult GetPreviousView<T>(FormSection section, string actionName, T model)
        {
            if (!FormDefinition.CanViewPage(section, actionName, model))
            {
                return RedirectToPreviousPossibleView(section, actionName, model);
            }

            var viewModel = FormDefinition.GetViewModel(section, actionName, model);

            return View(actionName, viewModel);
        }

        [System.Obsolete]
        protected ActionResult RedirectToPreviousPossibleView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            while (!FormDefinition.CanViewPage(section, id, model))
                id--;

            return RedirectBackToAction(section, id);
        }

        /// <summary>
        /// Returns a redirect action for the first view previous to the supplied view that 
        /// can be viewed based on the state of the parent view model.
        /// </summary>
        /// <typeparam name="T">The type of the parent view model.</typeparam>
        /// <param name="section">The forms section.</param>
        /// <param name="actionName">The name of the action to look for views before.</param>
        /// <param name="model">The parent view model.</param>
        /// <returns>A redirect <see cref="RedirectToActionResult"/> for the previous possible view with the <code>back</code> indicator.</returns>
        protected IActionResult RedirectToPreviousPossibleView<T>(FormSection section, string actionName, T model)
        {
            var nextPage = FormDefinition.GetPreviousPossiblePage(section, actionName, model);

            return RedirectBackToAction(section, nextPage.ActionName);
        }

        [System.Obsolete]
        protected ActionResult GetNextView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, id, model))
                return RedirectToNextPossibleView(id, section, model);

            var viewPath = GetViewPath(section, id);
            var viewModel = FormDefinition.GetViewModel(section, id, model);

            return View(viewPath, viewModel);
        }

        /// <summary>
        /// Return a view for the requested action name. If this view can't be viewed, it will seek forwards 
        /// through the pages until one that can be viewed is found. If no view can be found then the last view 
        /// will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the parent view model.</typeparam>
        /// <param name="section">The form section.</param>
        /// <param name="actionName">The name of the action to get the view for.</param>
        /// <param name="model">The parent view model.</param>
        /// <returns>A <see cref="ViewResult"/> for the current view or a <see cref="RedirectToActionResult"/> for the next possible view.</returns>
        protected IActionResult GetNextView<T>(FormSection section, string actionName, T model) where T : IValidatable
        {
            if (!FormDefinition.CanViewPage(section, actionName, model))
            {
                return RedirectToNextPossibleView(section, actionName, model);
            }

            var viewModel = FormDefinition.GetViewModel(section, actionName, model);

            return View(actionName, viewModel);
        }

        [System.Obsolete]
        protected ActionResult RedirectToNextPossibleView<T>(int id, FormSection section, T model) where T : IValidatable
        {
            while (!FormDefinition.CanViewPage(section, id, model))
                id++;

            return RedirectToAction(section, id);
        }

        /// <summary>
        /// Returns a redirect action for the first view following the supplied view that can be viewed 
        /// based on the state of the parent view model.
        /// </summary>
        /// <typeparam name="T">The type of the parent view model.</typeparam>
        /// <param name="section">The forms section.</param>
        /// <param name="actionName">The name of the action to look for views after.</param>
        /// <param name="model">The parent view model.</param>
        /// <returns>A redirect <see cref="RedirectToActionResult"/> for the next possible view.</returns>
        protected IActionResult RedirectToNextPossibleView<T>(FormSection section, string actionName, T model)
            where T : IValidatable
        {
            var nextPage = FormDefinition.GetNextPossiblePage(section, actionName, model);

            return RedirectToAction(section, nextPage.ActionName);
        }

        [System.Obsolete]
        protected virtual ActionResult RedirectToAction(FormSection section, int id)
        {
            return RedirectToAction("Part", section.ToString(), new {id});
        }

        /// <summary>
        /// Returns a <see cref="RedirectToActionResult"/> for the specified action name.
        /// </summary>
        /// <param name="section">The form section.</param>
        /// <param name="actionName">The action name.</param>
        /// <returns>A <see cref="RedirectToActionResult"/> for the specified action name.</returns>
        protected virtual IActionResult RedirectToAction(FormSection section, string actionName)
        {
            return RedirectToAction(actionName, section.ToString());
        }

        [System.Obsolete]
        protected virtual ActionResult RedirectBackToAction(FormSection section, int id)
        {
            return RedirectToAction("Part", section.ToString(), new { id, back = true });
        }

        /// <summary>
        /// Returns a <see cref="RedirectToActionResult"/> for the specified action name with the 
        /// <code>back</code> direction indicator in the query string.
        /// </summary>
        /// <param name="section">The form section.</param>
        /// <param name="actionName">The action name.</param>
        /// <returns>A <see cref="RedirectToActionResult"/> for the specified action name with the <code>back</code> indicator.</returns>
        protected virtual IActionResult RedirectBackToAction(FormSection section, string actionName)
        {
            return RedirectToAction(actionName, section.ToString(), new {back = true});
        }

        [System.Obsolete]
        protected virtual ActionResult RedirectToLastAction(FormSection section)
        {
            return RedirectToAction("Part", section.ToString(), new {id = FormDefinition.GetSectionLength(section)});
        }

        /// <summary>
        /// Returns a <see cref="RedirectToActionResult"/> to the last action in the section.
        /// </summary>
        /// <param name="section">The form section.</param>
        /// <returns>A <see cref="RedirectToActionResult"/> for the last action in the section.</returns>
        protected virtual IActionResult RedirectToLastActionForNewSection(FormSection section)
        {
            return RedirectToAction(section, GetLastViewPathForNewSection(section));
        }
    }
}