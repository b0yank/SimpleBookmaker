namespace SimpleBookmaker.Web.Controllers
{
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.BetViewModels;
    using Services.Contracts;
    using Services.Models.Bet;
    using Services.Models.Tournament;
    using Services.Models.UserCoefficient;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    
    public class HomeController : BaseController
    {
        private const int homePageTournamentsCount = 3;
        private const int upcomingGamesbyTournamentSize = 5;
        private const int upcomingGamesBetCount = 5;

        private readonly IGameBetsService gameBets;
        private readonly IGamesService games;
        private readonly ITeamsService teams;
        private readonly ITournamentBetsService tournamentBets;
        private readonly ITournamentsService tournaments;
        private readonly IUserBetsService userBets;

        public HomeController(IGameBetsService gameBets,
            ITournamentBetsService tournamentBets,
            IGamesService games, 
            ITeamsService teams, 
            ITournamentsService tournaments,
            IUserBetsService userBets)
        {
            this.gameBets = gameBets;
            this.games = games;
            this.teams = teams;
            this.tournamentBets = tournamentBets;
            this.tournaments = tournaments;
            this.userBets = userBets;
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult Index()
        {
            var upcomingTournaments = this.tournaments.AllImportant(homePageTournamentsCount);

            var upcomingGamesByTournament = new Dictionary<TournamentListModel, ICollection<GameBasicCoefficientsListModel>>();

            foreach (var tournament in upcomingTournaments)
            {
                var upcomingGames = this.games.Upcoming(1, upcomingGamesbyTournamentSize, tournament.Id);

                if (upcomingGames.Count() > 0)
                {
                    upcomingGamesByTournament[tournament] = new List<GameBasicCoefficientsListModel>();

                    foreach (var game in upcomingGames)
                    {
                        var gameBasicCoefficients = this.gameBets.ByGameBasic(game.Id);

                        upcomingGamesByTournament[tournament].Add(gameBasicCoefficients);
                    }
                }
            }
            
            return View(upcomingGamesByTournament);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        [Authorize]
        public IActionResult AddToBetSlip(BetUnconfirmedModel model, string returnUrl = null)
        {
            var betSlip = GetBetSlip();

            if (betSlip == null)
            {
                betSlip = new SessionBetSlip();
            }

            var betsInSlip = betSlip.GetBets().ToDictionary(b => b.CoefficientId, b => b.BetType);

            var hasConflictingBets = this.userBets.SlipHasConflictingBets(betsInSlip, model.CoefficientId, model.BetType);

            if (hasConflictingBets == null)
            {
                ModelState.AddModelError("", ErrorMessages.InvalidCoefficient);
            }
            else if (hasConflictingBets == true)
            {
                ModelState.AddModelError("", ErrorMessages.ConflictingBets);
            }
            else
            {
                var result = betSlip.AddBet(model);

                if (!result.Success)
                {
                    foreach (var error in result.GetErrors())
                    {
                        ModelState.AddModelError("", error);
                    }

                }
            }

            SetBetSlip(betSlip);

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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
