using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GLAA.Web.Attributes
{
    //public class SessionTimeoutAttribute : ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var context = filterContext.HttpContext;
    //        if (context.Session != null)
    //        {
    //            if (context.Session.)
    //            {
    //                var sessionCookie = context.Request.Headers["Cookie"];
    //                if (sessionCookie != null && sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.InvariantCultureIgnoreCase) >= 0)
    //                {
    //                    if (context.User.Identities.Any(x => x.IsAuthenticated))
    //                    {
    //                        FormsAuthentication.SignOut();
    //                    }

    //                    var redirectTo = "~/Home/SessionTimeout";
    //                    filterContext.Result = new RedirectResult(redirectTo);
    //                }
    //            }
    //        }
    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}