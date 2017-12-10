namespace SimpleBookmaker.Web.Controllers
{
    using Extensions;
    using Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models.BetViewModels;
    using Services.Contracts;

    public class BaseController : Controller
    {
        protected readonly IGameBetsService gameBets;
        protected readonly ITournamentBetsService tournamentBets;

        public BaseController(IGameBetsService gameBets, ITournamentBetsService tournamentBets)
        {
            this.gameBets = gameBets;
            this.tournamentBets = tournamentBets;
        }

        [HttpPost]
        public IActionResult AddToBetSlip(BetUnconfirmedModel model, string returnUrl = null)
        {
            var betSlip = HttpContext.Session.Get<SessionBetSlip>(GlobalConstants.SessionBetSlipKey);

            if (betSlip == null)
            {
                betSlip = new SessionBetSlip();
            }

            betSlip.Bets.Add(model);

            HttpContext.Session.Set<SessionBetSlip>(GlobalConstants.SessionBetSlipKey, betSlip);

            return this.RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}