using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreManagement.Models
{
    public class ValidateSession : ActionFilterAttribute
    {
        public override  void OnActionExecuting(ActionExecutingContext context)
        {
            var ctx = context.HttpContext.Session.GetObjectFromJson<TM_User>("UserAccount");           
            if (ctx is null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
                context.Result.ExecuteResultAsync(context);
            }
            base.OnActionExecuting(context);
        }
    }
}
