namespace SimpleBookmaker.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Models.StatisticianViewModels;
    using Services.Contracts;
    using Services.Infrastructure;
    using Services.Infrastructure.EventStats;
    using Services.Models.Game;
    using Web.Infrastructure;
    using Web.Infrastructure.Filters;
    using System.Linq;
    using System.Collections.Generic;

    [Authorize(Roles = Roles.Statistician)]
    public class StatisticianController : Controller
    {
        private readonly IPlayersService players;
        private readonly ITeamsService teams;
        private readonly IStatisticianTournamentsService tournaments;
        private readonly IStatisticianGamesService games;

        public StatisticianController(IPlayersService players,
            ITeamsService teams,
            IStatisticianTournamentsService tournaments, 
            IStatisticianGamesService games)
        {
            this.games = games;
            this.teams = teams;
            this.tournaments = tournaments;
            this.players = players;
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult Index()
        {
            var pastTournaments = this.tournaments.Finished();
            var pastGames = this.games.Finished();

            var viewModel = new PastEventsViewModel
            {
                PastTournaments = pastTournaments,
                PastGames = pastGames
            };

            return View(viewModel);
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult SetStats(int gameId)
        {
            if (!this.games.Exists(gameId))
            {
                return BadRequest();
            }

            var game = this.games.ById(gameId);

            return View(game);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult SetStats(GameStatsModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(SetStats), new { gameId = model.Id });
            }

            var homeGoalcorers = model.HomeGoalscorers != null 
                ? model.HomeGoalscorers.Trim().Split(' ').Select(hg => int.Parse(hg))
                : new List<int>();

            var awayGoalcorers = model.AwayGoalscorers != null
                ? model.AwayGoalscorers.Trim().Split(' ').Select(hg => int.Parse(hg))
                : new List<int>();

            var gameStats = new GameStats(
                model.Id,
                model.HomeCorners,
                model.AwayCorners,
                model.HomeFreeKicks,
                model.AwayFreeKicks,
                model.HomePossession,
                model.AwayPossession,
                homeGoalcorers,
                awayGoalcorers);

            this.games.ResolveBets(gameStats);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult SetTournament(int tournamentId)
        {
            if (!this.tournaments.Exists(tournamentId))
            {
                return BadRequest();
            }

            var tournament = this.tournaments.ById(tournamentId);

            return View(tournament);
        }

        [HttpPost]
        public IActionResult SetTournament(TournamentStatsSaveModel model)
        {
            if (!this.tournaments.Exists(model.TournamentId))
            {
                return BadRequest();
            }

            if (!this.teams.Exists(model.ChampionId))
            {
                return BadRequest();
            }

            var success = this.tournaments.ResolveBets(model.TournamentId, model.ChampionId);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.TournamentRemoveFailed);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
