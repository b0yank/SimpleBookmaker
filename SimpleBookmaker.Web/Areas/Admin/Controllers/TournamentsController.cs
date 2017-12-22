namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Infrastructure;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models.Tournament;
    using Services.Contracts;
    using Services.Models.Tournament;
    using System;
    using Web.Models.TournamentViewModels;
    
    public class TournamentsController : AdminBaseController
    {
        private const int tournamentListPageSize = 20;
        private const int gamesListPageSize = 20;

        private ITournamentsService tournaments;

        public TournamentsController(ITournamentsService tournaments)
        {
            this.tournaments = tournaments;
        }
        
        [RestoreModelErrorsFromTempData]
        public IActionResult All(int page = 1)
        {
            var allTournaments = this.tournaments.AllDetailed(page, tournamentListPageSize);

            var tournamentsCount = this.tournaments.Count();

            var viewModel = new TournamentDetailedListPageModel
            {
                Tournaments = allTournaments,
                CurrentPage = page,
                TotalPages = (int) Math.Ceiling(tournamentsCount / (double)tournamentListPageSize),
                RequestPath = "admin/tournaments/all"
            };

            return View(viewModel);
        }
        
        public IActionResult Add() => View();
        
        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult Add(TournamentAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = this.tournaments.Add(model.Name, model.StartDate, model.EndDate, model.Importance);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.TournamentAddFailed);
            }

            return this.RedirectToAction(nameof(All));
        }
        
        public IActionResult AddTeam(int tournamentId)
        {
            var tournamentName = this.tournaments.GetName(tournamentId);

            if (tournamentName == null)
            {
                return BadRequest();
            }

            var availableTeams = this.tournaments.GetAvailableTeams(tournamentId);

            var viewModel = new TournamentAddTeamViewModel
            {
                TournamentId = tournamentId,
                TournamentName = tournamentName,
                AvailableTeams = availableTeams
            };

            return View(viewModel);
        }

        public IActionResult Edit(int tournamentId)
        {
            if (!this.tournaments.Exists(tournamentId))
            {
                return BadRequest();
            }

            var tournament = this.tournaments.ById(tournamentId);

            return View(tournament);
        }

        [HttpPost]
        public IActionResult Edit(TournamentModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = this.tournaments.Edit(model.Id, model.Name, model.StartDate, model.EndDate);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Remove(int tournamentId)
        {
            if (!this.tournaments.Exists(tournamentId))
            {
                return BadRequest();
            }

            var tournament = this.tournaments.GetName(tournamentId);

            var model = new TournamentListModel
            {
                Id = tournamentId,
                Name = tournament
            };

            return View(model);
        }

        [HttpPost]
        [SetTempDataModelErrors]
        public IActionResult Remove(TournamentRemoveModel model)
        {
            if (!this.tournaments.Exists(model.Id))
            {
                return BadRequest();
            }

            var success = this.tournaments.Remove(model.Id);

            if (!success)
            {
                ModelState.AddModelError("", ErrorMessages.TournamentRemoveFailed);
            }

            return RedirectToAction(nameof(All));
        }
        
        [HttpPost]
        public IActionResult AddTeam(TournamentAddTeamBindingModel model)
        {
            if (!this.tournaments.Exists(model.TournamentId))
            {
                return BadRequest();
            }

            foreach (var teamId in model.Teams)
            {
                if (this.tournaments.IsInTournament(model.TournamentId, teamId))
                {
                    return BadRequest();
                }
            }

            var success = this.tournaments.AddTeams(model.TournamentId, model.Teams);

            if (!success)
            {
                return BadRequest();
            }

            return RedirectToAction(nameof(All));
        }
        
        [RestoreModelErrorsFromTempData]
        public IActionResult Games(int tournamentId, int page = 1)
        {
            var tournament = this.tournaments.GetName(tournamentId);

            if (tournament == null)
            {
                return BadRequest();
            }

            var tournamentGames = this.tournaments.Games(tournamentId, false, page, gamesListPageSize);

            var gamesCount = this.tournaments.GamesCount(tournamentId);

            var viewModel = new TournamentGamesListPageModel
            {
                TournamentId = tournamentId,
                Tournament = tournament,
                Games = tournamentGames,
                CurrentPage = page,
                TotalPages = (int) Math.Ceiling(gamesCount / (double)gamesListPageSize),
                RequestPath = $"admin/tournaments/games/{tournamentId}"
            };

            return View(viewModel);
        }
    }
}