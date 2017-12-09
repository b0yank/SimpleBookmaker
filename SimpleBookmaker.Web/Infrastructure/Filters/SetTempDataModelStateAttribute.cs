namespace SimpleBookmaker.Web.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using System.Linq;

    public class SetTempDataModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var controller = ((Controller)filterContext.Controller);

            var errors = controller.ViewData.ModelState.Values.SelectMany(ms => ms.Errors.Select(e => e.ErrorMessage));

            controller.TempData["ModelState"] = JsonConvert
                .SerializeObject(errors);
        }
    }
}