namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Infrastructure;

    [Area("Admin")]
    [Authorize(Roles = Roles.Administrator)]
    [Route("[area]/[controller]/[action]")]
    public abstract class AdminBaseController : Controller
    {
    }
}