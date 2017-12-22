using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GLAA.Web.Attributes
{
    // https://www.exceptionnotfound.net/the-post-redirect-get-pattern-in-asp-net-mvc/
    public abstract class ModelStateTransferAttribute : ActionFilterAttribute
    {
        protected static readonly string Key = typeof(ModelStateTransferAttribute).FullName;
    }

    public class ExportModelStateAttribute : ModelStateTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            if (!controller.ViewData.ModelState.IsValid)
            {
                if (filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult)
                {
                    controller.TempData[Key] = controller.ViewData.ModelState;
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }

    public class ImportModelStateAttribute : ModelStateTransferAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller as Controller;
            var modelState = controller.TempData[Key] as ModelStateDictionary;

            if (modelState != null)
            {
                if (filterContext.Result is ViewResult)
                {
                    controller.ViewData.ModelState.Merge(modelState);
                }
                else
                {
                    controller.TempData.Remove(Key);
                }
            }

            base.OnActionExecuted(filterContext);
        }
    }
}