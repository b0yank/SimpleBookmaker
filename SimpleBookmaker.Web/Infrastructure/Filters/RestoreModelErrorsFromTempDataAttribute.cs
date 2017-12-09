namespace SimpleBookmaker.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json.Linq;

    public class RestoreModelErrorsFromTempDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var controller = (Controller)(context.Controller);

            if (controller.TempData["Errors"] != null
                && (controller.TempData["Errors"] as string[]) != null
                && ((string[])controller.TempData["Errors"]).Length > 0)
            {
                var errors = (string[])controller.TempData["Errors"];

                foreach (var error in errors)
                {
                    controller.ModelState.AddModelError("", error);
                }
            }
        }
    }
}
