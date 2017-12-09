namespace SimpleBookmaker.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System.Linq;

    public class SetTempDataModelErrorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            var controller = (Controller)context.Controller;

            var errors = controller
                .ModelState
                .Values
                .SelectMany(ms => ms.Errors
                    .Select(e => e.ErrorMessage))
                .ToList();

            controller.TempData["Errors"] = errors;
        }
    }
}
