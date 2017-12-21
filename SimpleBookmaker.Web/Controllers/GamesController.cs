namespace SimpleBookmaker.Web.Controllers
{
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models.GameViewModels;
    using Services.Contracts;
    using System;

    public class GamesController : Controller
    {
        private readonly IGameBetsService gameBets;
        private readonly IUserBetsService userBets;
        private readonly IGamesService games;

        public GamesController(IGameBetsService gameBets, IUserBetsService userBets, IGamesService games)
        {
            this.gameBets = gameBets;
            this.userBets = userBets;
            this.games = games;
        }

        [RestoreModelErrorsFromTempData]
        public IActionResult View(int id)
        {
            if (!this.games.Exists(id))
            {
                return BadRequest();
            }

            var gameCoefficients = this.userBets.GameCoefficients(id);
            var gamePlayerCoefficents = this.gameBets.ExistingGamePlayerCoefficients(id);

            var teams = this.games.GetGameTeams(id);
            var gameTime = this.games.GetGametime(id);

            var viewModel = new GameCoefficientsViewModel
            {
                GameCoefficients = gameCoefficients,
                PlayerCoefficients = gamePlayerCoefficents,
                HomeTeam = teams.HomeTeam,
                AwayTeam = teams.AwayTeam,
                GameTime = (DateTime) gameTime
            };

            return View(viewModel);
        }
    }
}
