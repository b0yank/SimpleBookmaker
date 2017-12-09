﻿namespace SimpleBookmaker.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Tournament;
    using Services.Contracts;
    using Services.Models.Tournament;
    using SimpleBookmaker.Web.Infrastructure;
    using System;
    
    public class TournamentsController : AdminBaseController
    {
        private const int gamesListPageSize = 20;

        private ITournamentsService tournaments;

        public TournamentsController(ITournamentsService tournaments)
        {
            this.tournaments = tournaments;
        }
        
        public IActionResult All()
        {
            var allTournaments = this.tournaments.AllDetailed();

            return View(allTournaments);
        }
        
        public IActionResult Add() => View();
        
        [HttpPost]
        public IActionResult Add(TournamentAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.tournaments.Add(model.Name, model.StartDate, model.EndDate);

            return this.RedirectToAction(nameof(All));
        }
        
        public IActionResult AddTeam(int tournamentId)
        {
            var tournamentName = this.tournaments.GetName(tournamentId);

            if (tournamentName == null)
            {
                return NotFound(ErrorMessages.InvalidTournament);
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
                return NotFound(ErrorMessages.InvalidTournament);
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
                return NotFound(ErrorMessages.InvalidTournament);
            }

            return RedirectToAction(nameof(All));
        }

        public IActionResult Remove(int tournamentId)
        {
            if (!this.tournaments.Exists(tournamentId))
            {
                return NotFound(ErrorMessages.InvalidTournament);
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
        public IActionResult Remove(TournamentRemoveModel model)
        {
            if (!this.tournaments.Exists(model.Id))
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            var success = this.tournaments.Remove(model.Id);

            if (!success)
            {
                return NotFound(ErrorMessages.TournamentRemoveFailed);
            }

            return RedirectToAction(nameof(All));
        }
        
        [HttpPost]
        public IActionResult AddTeam(TournamentAddTeamBindingModel model)
        {
            if (!this.tournaments.Exists(model.TournamentId))
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            foreach (var teamId in model.Teams)
            {
                if (this.tournaments.IsInTournament(model.TournamentId, teamId))
                {
                    return BadRequest(ErrorMessages.TeamAlreadyInTournament);
                }
            }

            var success = this.tournaments.AddTeams(model.TournamentId, model.Teams);

            if (!success)
            {
                return NotFound(ErrorMessages.TeamTournamentAddFailed);
            }

            return RedirectToAction(nameof(All));
        }
        
        public IActionResult Games(int tournamentId, int page = 1)
        {
            var tournament = this.tournaments.GetName(tournamentId);

            if (tournament == null)
            {
                return NotFound(ErrorMessages.InvalidTournament);
            }

            var tournamentGames = this.tournaments.Games(tournamentId, false, page, gamesListPageSize);

            var gamesCount = this.tournaments.GamesCount(tournamentId);

            var viewModel = new TournamentGamesListPageModel
            {
                TournamentId = tournamentId,
                Tournament = tournament,
                Games = tournamentGames,
                CurrentPage = page,
                TotalPages = (int) Math.Ceiling(gamesCount / (double)gamesListPageSize)
            };

            return View(viewModel);
        }
    }
}