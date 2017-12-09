namespace SimpleBookmaker.Web.Areas.Bookmaker.Controllers
{
    using Data.Core.Enums;
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Models.Game;
    using Services.Contracts;
    using Services.Models.Game;
    using System.Collections.Generic;

    public class GamesController : BookmakerBaseController
    {
        private const int gameListPageSize = 30;

        private IBetsService bets;
        private IGamesService games;
        private ITeamsService teams;
        private ITournamentsService tournaments;
        private IPlayersService players;

        public GamesController(IBetsService bets, 
            ITeamsService teams, 
            IGamesService games, 
            ITournamentsService tournaments,
            IPlayersService players)
        {
            this.bets = bets;
            this.games = games;
            this.teams = teams;
            this.tournaments = tournaments;
            this.players = players;
        }

        public IActionResult All(int tournamentId = 0, int page = 1)
        {
            IEnumerable<GameListModel> games;
            if (tournamentId != 0)
            {
                games = this.tournaments.Games(tournamentId, true, page, gameListPageSize);
            }
            else
            {
                games = this.games.Upcoming(page, gameListPageSize);
            }

            var allTournaments = this.tournaments.All();

            var viewModel = new BookieGameListPageModel
            {
                Games = games,
                Tournaments = allTournaments,
                CurrentPage = page,
                TotalPages = this.games.UpcomingCount(tournamentId)
            };

            return View(viewModel);
        }
        
        [RestoreModelErrorsFromTempData]
        public IActionResult SetBets(int gameId)
        {
            if (!this.games.Exists(gameId))
            {
                return NotFound(ErrorMessages.InvalidGame);
            }

            var existingCoefficients = this.bets.ExistingGameCoefficients(gameId);
            var possibleCoefficients = this.bets.PossibleGameCoefficients(gameId);
            var teams = this.games.GetTeams(gameId);


            var viewModel = new SetBetsViewModel
            {
                GameId = gameId,
                HomeTeam = teams.HomeTeam,
                AwayTeam = teams.AwayTeam,
                ExistingCoefficients = existingCoefficients,
                PossibleCoefficients = possibleCoefficients
            };

            return View(viewModel);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult SetBets(GameCoefficientEditModel model)
        {
            if (ModelState.IsValid)
            {
                var success = this.bets.AddGameCoefficient(model.GameId, model.BetType, model.Side, model.Coefficient, model.HomeGoals, model.AwayGoals);

                if (!success)
                {
                    ModelState.AddModelError("", "Coefficient for this outcome already exists.");
                }
            }

            return RedirectToAction(nameof(SetBets), new { gameId = model.GameId });
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult SetPlayerBets(int gameId)
        {
            if (!this.games.Exists(gameId))
            {
                return NotFound(ErrorMessages.InvalidGame);
            }

            var existingCoefficients = this.bets.ExistingGamePlayerCoefficients(gameId);
            var possibleCoefficients = this.bets.PossibleGamePlayerCoefficients();

            var teams = this.games.GetTeams(gameId);
            var homePlayers = this.players.ByTeam(teams.HomeTeam);
            var awayPlayers = this.players.ByTeam(teams.AwayTeam);

            var viewModel = new SetPlayerBetsViewModel
            {
                GameId = gameId,
                ExistingCoefficients = existingCoefficients,
                PossibleCoefficients = possibleCoefficients,
                HomeTeam = teams.HomeTeam,
                AwayTeam = teams.AwayTeam,
                HomePlayers = homePlayers,
                AwayPlayers = awayPlayers
            };

            return View(viewModel);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult SetPlayerBets(SavePlayerGameCoefficientModel model)
        {
            if (!this.games.Exists(model.GameId))
            {
                return NotFound(ErrorMessages.InvalidGame);
            }

            if (!this.players.Exists(model.PlayerId))
            {
                return NotFound(ErrorMessages.InvalidPlayer);
            }

            var success = this.bets.AddPlayerGameCoefficient(model.PlayerId, 
                model.GameId, 
                model.BetType, 
                model.Coefficient);

            if (!success)
            {
                ModelState.AddModelError("", "Coefficient for this outcome already exists.");
            }

            return RedirectToAction(nameof(SetPlayerBets), new { gameId = model.GameId });
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult EditCoefficient(EditCoefficientModel model)
        {
            if (ModelState.IsValid)
            {
                this.bets.EditCoefficient(model.CoefficientId, model.NewCoefficient, BetType.Game);
            }

            return RedirectToAction(nameof(SetBets), new { gameId = model.SubjectId });
        }

        [HttpPost]
        public IActionResult RemoveCoefficient(RemoveCoefficientModel model)
        {
            this.bets.RemoveCoefficient(model.CoefficientId, BetType.Game);

            return RedirectToAction(nameof(SetBets), new { gameId = model.SubjectId });
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult EditPlayerCoefficient(EditCoefficientModel model)
        {
            if (ModelState.IsValid)
            {
                this.bets.EditCoefficient(model.CoefficientId, model.NewCoefficient, BetType.PlayerGame);
            }

            return RedirectToAction(nameof(SetPlayerBets), new { gameId = model.SubjectId });
        }

        [HttpPost]
        public IActionResult RemovePlayerCoefficient(RemoveCoefficientModel model)
        {
            this.bets.RemoveCoefficient(model.CoefficientId, BetType.PlayerGame);

            return RedirectToAction(nameof(SetPlayerBets), new { gameId = model.SubjectId });
        }
    }
}