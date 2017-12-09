namespace SimpleBookmaker.Web.Areas.Bookmaker.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Infrastructure;

    [Area("Bookmaker")]
    [Route("[area]/[controller]/[action]")]
    [Authorize(Roles = Roles.Bookmaker)]
    public abstract class BookmakerBaseController : Controller
    {
    }
}