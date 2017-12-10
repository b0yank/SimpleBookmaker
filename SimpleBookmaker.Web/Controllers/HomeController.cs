namespace SimpleBookmaker.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Services.Contracts;
    using Services.Models.Coefficient;
    using Services.Models.Tournament;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class HomeController : BaseController
    {
        private const int homePageTournamentsCount = 3;
        private const int upcomingGamesbyTournamentSize = 5;
        private const int upcomingGamesBetCount = 5;
        
        private readonly IGamesService games;
        private readonly ITeamsService teams;
        private readonly ITournamentsService tournaments;

        public HomeController(IGameBetsService gameBets,
            ITournamentBetsService tournamentBets,
            IGamesService games, 
            ITeamsService teams, 
            ITournamentsService tournaments)
            : base(gameBets, tournamentBets)
        {
            this.games = games;
            this.teams = teams;
            this.tournaments = tournaments;
        }

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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
