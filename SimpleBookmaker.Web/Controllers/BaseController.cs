namespace SimpleBookmaker.Web.Controllers
{
    using Extensions;
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.BetViewModels;
    using Services.Contracts;
    using Services.Models.Bet;
    using System.Linq;

    public class BaseController : Controller
    {
        protected SessionBetSlip GetBetSlip()
            => HttpContext.Session.Get<SessionBetSlip>(GlobalConstants.SessionBetSlipKey);

        protected void SetBetSlip(SessionBetSlip betSlip)
            => HttpContext.Session.Set<SessionBetSlip>(GlobalConstants.SessionBetSlipKey, betSlip);
    }
}