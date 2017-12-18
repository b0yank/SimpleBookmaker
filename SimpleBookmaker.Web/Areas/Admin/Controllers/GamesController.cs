namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models.Game;
    using Services.Contracts;
    using System;
    
    public class GamesController : AdminBaseController
    {
        private readonly IGamesService games;
        private readonly ITeamsService teams;
        private readonly ITournamentsService tournaments;

        public GamesController(IGamesService games, ITeamsService teams, ITournamentsService tournaments)
        {
            this.games = games;
            this.teams = teams;
            this.tournaments = tournaments;
        }
        
        public IActionResult ChooseTournament()
        {
            var allTournaments = this.tournaments.All();

            return View(allTournaments);
        }
        
        public IActionResult Add(int tournamentId)
        {
            if (!this.tournaments.Exists(tournamentId))
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            var viewModel = new GameAddModel
            {
                TournamentId = tournamentId
            };
            
            return View(viewModel);
        }
        
        [HttpPost]
        public IActionResult Add(GameAddModel model)
        {
            DateTime dateTime = model.Date;

            dateTime = dateTime.AddHours(model.Time.Hour);
            dateTime = dateTime.AddMinutes(model.Time.Minute);

            if (!this.tournaments.IsDuringTournament(model.TournamentId, dateTime))
            {
                ModelState.AddModelError("", "Game must be held in the duration of the tournament");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = this.games.Add(model.TournamentId, model.HomeTeamId, model.AwayTeamId, dateTime);

            if (!success)
            {
                return NotFound(ErrorMessages.GameCreateFailed);
            }

            return RedirectToAction(nameof(TournamentsController.Games), "Tournaments", new { tournamentId = model.TournamentId });
        }
        
        public IActionResult Remove(int id)
        {
            var game = this.games.ById(id);

            if (game == null)
            {
                return NotFound(ErrorMessages.InvalidGame);
            }

            return View(game);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult Remove(GameDestroyModel model)
        {
            var success = this.games.Remove(model.Id);

            if (!success)
            {
                ModelState.AddModelError("", "Game could not be removed - perhaps there are unresolved bets?");
            }

            return RedirectToAction(nameof(TournamentsController.Games), "Tournaments" , new { tournamentId = model.TournamentId } );
        }
        
        public IActionResult Edit(int id)
        {
            var game = this.games.ById(id);

            return View(game);
        }

        [HttpPost]
        public IActionResult Edit(GameEditModel model)
        {
            DateTime newKickoff = model.Date;

            newKickoff = newKickoff.AddHours(model.Time.Hour);
            newKickoff = newKickoff.AddMinutes(model.Time.Minute);

            if (!this.tournaments.IsDuringTournament(model.TournamentId, model.Date))
            {
                ModelState.AddModelError("", "Game must be held in the duration of the tournament");
            }

            if (!ModelState.IsValid)
            {
                var viewModel = this.games.ById(model.Id);
                viewModel.Kickoff = newKickoff;

                return View(viewModel);
            }

            this.games.Edit(model.Id, newKickoff);

            return RedirectToAction(nameof(TournamentsController.Games), "Tournaments", new { tournamentId = model.TournamentId });
        }
    }
}
